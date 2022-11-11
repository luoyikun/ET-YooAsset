using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Compilation;
using UnityEngine;

namespace ET
{
    [InitializeOnLoad]
    public static class BuildAssemblieEditor
    {
        private static bool s_CompileHotfixCompleted = false;
        private static bool m_IsContainsNkgEditorOnlySymbolDefine = false;

        private static string assemblyName;
        private static string[] includeAssemblies;
        private static string[] additionalReferences;
        private static CodeOptimization codeOptimization;
        private static string[] backSymbolDefines;

        private static UniTaskCompletionSource s_UniTaskCompletionSource;

        /// <summary>
        /// 自动进行dll拷贝操作，避免每次都需要手动编译，但需要注意的是，每次打包必须进行一次手动编译
        /// </summary>
        static BuildAssemblieEditor()
        {
            File.Copy(Path.Combine(GlobalDefine.UnityAutoCompiledHotfixDllDir, "ProjectS_Hotfix.dll"),
                Path.Combine(GlobalDefine.CodeDir, "ProjectS_Hotfix.dll.bytes"), true);
            File.Copy(Path.Combine(GlobalDefine.UnityAutoCompiledHotfixDllDir, "ProjectS_Hotfix.pdb"),
                Path.Combine(GlobalDefine.CodeDir, "ProjectS_Hotfix.pdb.bytes"), true);

            AssetDatabase.Refresh();
            Log.Info($"自动从{GlobalDefine.UnityAutoCompiledHotfixDllDir}拷贝ProjectS_Hotfix.dll到项目里，已完成，但正式出包一定进行一次手动编译");
        }

        [MonKey.Command("Build Hotfix Debug Dll", "编译热更DLL（Debug）", Category = "Build")]
        public static UniTask BuildCodeDebug()
        {
            assemblyName = "ProjectS_Hotfix";
            includeAssemblies = new[]
            {
                "Packages/ProjectS_Hotfix/Model/",
                "Packages/ProjectS_Hotfix/ModelView/",
                "Packages/ProjectS_Hotfix/Hotfix/",
                "Packages/ProjectS_Hotfix/HotfixView/"
            };
            additionalReferences = Array.Empty<string>();
            codeOptimization = CodeOptimization.Debug;

            return BuildAssemblieEditor.BuildMuteAssembly();
        }

        [MonKey.Command("Build Hotfix Release Dll", "编译热更DLL（Release）", Category = "Build")]
        public static UniTask BuildCodeRelease()
        {
            assemblyName = "ProjectS_Hotfix";
            includeAssemblies = new[]
            {
                "Packages/ProjectS_Hotfix/Model/",
                "Packages/ProjectS_Hotfix/ModelView/",
                "Packages/ProjectS_Hotfix/Hotfix/",
                "Packages/ProjectS_Hotfix/HotfixView/"
            };
            additionalReferences = Array.Empty<string>();
            codeOptimization = CodeOptimization.Release;

            return BuildAssemblieEditor.BuildMuteAssembly();
        }

        [MonKey.Command("Build Hotfix Data Dll", "编译热更DLL（Debug）", Category = "Build")]
        public static UniTask BuildData()
        {
            assemblyName = "ProjectS_Hotfix_Data";
            includeAssemblies = new[]
            {
                "Packages/ProjectS_Hotfix/Model/",
                "Packages/ProjectS_Hotfix/ModelView/",
            };
            additionalReferences = Array.Empty<string>();
            codeOptimization = CodeOptimization.Debug;

            return BuildAssemblieEditor.BuildMuteAssembly();
        }


        [MonKey.Command("Build Hotfix Logic Dll", "编译热更DLL（Debug）", Category = "Build")]
        public static UniTask BuildLogic()
        {
            assemblyName = "ProjectS_Hotfix_Logic";
            includeAssemblies = new[]
            {
                "Packages/ProjectS_Hotfix/Hotfix/",
                "Packages/ProjectS_Hotfix/HotfixView/"
            };
            additionalReferences = Array.Empty<string>();
            codeOptimization = CodeOptimization.Debug;

            return BuildAssemblieEditor.BuildMuteAssembly();
        }

        private static UniTask BuildMuteAssembly()
        {
            s_UniTaskCompletionSource = new UniTaskCompletionSource();

            s_CompileHotfixCompleted = false;

            List<string> scripts = new List<string>();
            for (int i = 0; i < includeAssemblies.Length; i++)
            {
                DirectoryInfo dti = new DirectoryInfo(includeAssemblies[i]);
                FileInfo[] fileInfos = dti.GetFiles("*.cs", System.IO.SearchOption.AllDirectories);
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    scripts.Add(fileInfos[j].FullName);
                }
            }

            string dllPath = Path.Combine(GlobalDefine.BuildOutputDir, $"{assemblyName}.dll");
            string pdbPath = Path.Combine(GlobalDefine.BuildOutputDir, $"{assemblyName}.pdb");
            if (Directory.Exists(GlobalDefine.BuildOutputDir))
            {
                File.Delete(dllPath);
                File.Delete(pdbPath);
            }
            else
                Directory.CreateDirectory(GlobalDefine.BuildOutputDir);

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(dllPath, scripts.ToArray());

            //启用UnSafe
            //assemblyBuilder.compilerOptions.AllowUnsafeCode = true;

            BuildTargetGroup buildTargetGroup =
                BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

            assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
            assemblyBuilder.compilerOptions.ApiCompatibilityLevel =
                PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
            // assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;

            assemblyBuilder.additionalReferences = additionalReferences;

            assemblyBuilder.flags = AssemblyBuilderFlags.None;
            //AssemblyBuilderFlags.None                 正常发布
            //AssemblyBuilderFlags.DevelopmentBuild     开发模式打包
            //AssemblyBuilderFlags.EditorAssembly       编辑器状态
            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;

            assemblyBuilder.buildTargetGroup = buildTargetGroup;

            assemblyBuilder.excludeReferences = new string[]
            {
                "Library/ScriptAssemblies/Unity.Editor.dll", "Library/ScriptAssemblies/ProjectS_Hotfix.dll"
            };

            assemblyBuilder.buildStarted += delegate(string assemblyPath)
            {
                Debug.LogFormat("build start：" + assemblyPath);
            };

            assemblyBuilder.buildFinished += delegate(string assemblyPath, CompilerMessage[] compilerMessages)
            {
                int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

                Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);

                if (warningCount > 0)
                {
                    Debug.LogFormat("有{0}个Warning!!!", warningCount);
                }

                if (errorCount > 0)
                {
                    for (int i = 0; i < compilerMessages.Length; i++)
                    {
                        if (compilerMessages[i].type == CompilerMessageType.Error)
                        {
                            Debug.LogError(compilerMessages[i].message);
                        }
                    }
                }

                s_CompileHotfixCompleted = true;

                if (m_IsContainsNkgEditorOnlySymbolDefine)
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                        backSymbolDefines.ToArray());
                }
            };

            //assemblyBuilder.excludeReferences = new string[] { "Unity.Editor.dll" };

            EditorApplication.update += CheckCompileHotfixCompleted;

            //开始构建
            if (!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("build fail：" + assemblyBuilder.assemblyPath);
            }

            return s_UniTaskCompletionSource.Task;
        }

        private static void CheckCompileHotfixCompleted()
        {
            if (!s_CompileHotfixCompleted)
            {
                EditorUtility.DisplayProgressBar("正在编译热更程序集，请稍等。。。", "Wait...", 1.0f);
                return;
            }

            EditorUtility.ClearProgressBar();

            EditorApplication.update -= CheckCompileHotfixCompleted;
            s_CompileHotfixCompleted = false;

            Debug.Log("Compiling finish");

            Directory.CreateDirectory(GlobalDefine.CodeDir);

            File.Copy(Path.Combine(GlobalDefine.BuildOutputDir, "ProjectS_Hotfix.dll"),
                Path.Combine(GlobalDefine.CodeDir, "ProjectS_Hotfix.dll.bytes"), true);
            File.Copy(Path.Combine(GlobalDefine.BuildOutputDir, "ProjectS_Hotfix.pdb"),
                Path.Combine(GlobalDefine.CodeDir, "ProjectS_Hotfix.pdb.bytes"), true);

            AssetDatabase.Refresh();
            Debug.Log("copy Code.dll to Bundles/Code success!");

            s_UniTaskCompletionSource.TrySetResult();
        }
    }
}