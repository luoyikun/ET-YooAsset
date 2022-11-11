// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年6月29日, 星期三
// --------------------------

using System;
using System.Collections.Generic;
using System.IO;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using SimpleJSON;
using Sirenix.Serialization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using YooAsset;

namespace ET
{
    public static class BuildStandalonePlayer
    {
        [MonKey.Command("Build EXE", "打出EXE文件，并刷新用于AOT元数据补充的DLL", Category = "Build")]
        public static void Build()
        {
            PrebuildCommand.GenerateAll();
            
            var outputPath =
                $"{GlobalDefine.RelativeDirPrefix}/ProjectS_EXE"; //EditorUtility.SaveFolderPanel("Choose Location of the Built Game", "", "");
            if (outputPath.Length == 0)
                return;

            #region 将Unity的BuildInScene设置为仅包含Init，因为我们为了支持在编辑器模式下的测试而必须将所有Scene放到Unity的BuildInSetting里

            var backScenes = EditorBuildSettings.scenes;
            var scenes = new EditorBuildSettingsScene[] { new EditorBuildSettingsScene(GlobalDefine.InitScenePath, true) };
            EditorBuildSettings.scenes = scenes;

            #endregion

            EditorSceneManager.OpenScene(GlobalDefine.InitScenePath);
            
            // 如果执行打包，就强行替换为非本地调试模式，进行AB加载
            Init updater = Init.Instance;
            YooAssets.EPlayMode backPlayMode = updater.PlayMode;
            updater.PlayMode = YooAssets.EPlayMode.HostPlayMode;

            var targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;

            var buildPlayerOptions = new BuildPlayerOptions
            {
                locationPathName = outputPath + targetName,
                target = EditorUserBuildSettings.activeBuildTarget,
                options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);

            updater.PlayMode = backPlayMode;
            EditorBuildSettings.scenes = backScenes;

            // 将AOT热更元数据补充dll复制到项目中
            var dstPath = SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);

            DLLNameListForAOT dllNameListForAOT = SerializationUtility.DeserializeValue<DLLNameListForAOT>( AssetDatabase
                .LoadAssetAtPath<TextAsset>("Assets/Res/OtherNativeRes/DLLNameListForAOT.json").bytes, DataFormat.JSON);
            
            foreach (var dllName in dllNameListForAOT.DLLNameList_Raw)
            {
                string targetDllFullName = $"{dstPath}/{dllName}";
                if (File.Exists(targetDllFullName))
                {
                    File.Copy($"{targetDllFullName}", $"Assets/Res/Code/{dllName}.bytes", true);
                }
            }
            
            AssetDatabase.Refresh();
        }

        private static string GetBuildTargetName(BuildTarget target)
        {
            var time = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var name = PlayerSettings.productName + "-v" + PlayerSettings.bundleVersion + ".";
            switch (target)
            {
                case BuildTarget.Android:
                    return string.Format("/{0}{1}-{2}.apk", name, 1, time);

                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return string.Format("/{0}{1}-{2}.exe", name, 1, time);

#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return "/" + name + ".app";

#else
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    return "/" + path + ".app";

#endif

                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    return "";
                // Add more build targets for your own.
                default:
                    Debug.Log("Target not implemented.");
                    return null;
            }
        }
    }
}