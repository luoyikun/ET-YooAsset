// Administrator20222311:502022

using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using YooAsset.Editor;

namespace ET
{
    public struct CIHelper
    {
        #region 流水线调用

        // 先构建热更dll，再构建ab
        private static UniTask BuildHotfixDll_AB()
        {
            return InternalBuild();

            async UniTask InternalBuild()
            {
                await BuildAssemblieEditor.BuildCodeRelease();

                BuildAB();

                EditorApplication.Exit(0);
            }
        }

        public static void BuildExe()
        {
            BuildStandalonePlayer.Build();
        }

        /// <summary>
        /// 构建EXE和AB和热更dll
        /// </summary>
        public static UniTask BuildAll()
        {
            return InternalBuildAll();

            async UniTask InternalBuildAll()
            {
                BuildExe();

                await BuildHotfixDll_AB();

                // 需要注意的是，一旦使用了UniTask，在batchmode需要自己处理Exit
                EditorApplication.Exit(0);
            }
        }

        /// <summary>
        /// 收集shader变体
        /// </summary>
        public static void CollectSVC()
        {
            // 先删除再保存，否则ShaderVariantCollection内容将无法及时刷新
            AssetDatabase.DeleteAsset(ShaderVariantCollectorSettingData.Setting.SavePath);

            ShaderVariantCollector.Run(ShaderVariantCollectorSettingData.Setting.SavePath, () =>
            {
                ShaderVariantCollection collection =
                    AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(ShaderVariantCollectorSettingData.Setting
                        .SavePath);

                if (collection != null)
                {
                    Debug.Log($"ShaderCount : {collection.shaderCount}");
                    Debug.Log($"VariantCount : {collection.variantCount}");
                }

                EditorSceneManager.OpenScene(GlobalDefine.InitScenePath);
                EditorTools.CloseUnityGameWindow();
                EditorApplication.Exit(0);
            });
        }

        #endregion


        #region 内部调用

        private static void BuildAB()
        {
            Debug.Log($"开始构建AB : {EditorUserBuildSettings.activeBuildTarget}");

            // 命令行参数 -buildABVersion 为AB版本，一般以流水线构建版本号为准
            int buildVersion = GetBuildVersion();

            if (buildVersion < 1)
            {
                Debug.LogError("未正确填写-buildABVersion参数，示例-buildABVersion %build.number%");
                return;
            }

            // 从构建命令里获取参数
            static int GetBuildVersion()
            {
                static string GetArg(string name)
                {
                    var args = System.Environment.GetCommandLineArgs();
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == name && args.Length > i + 1)
                        {
                            return args[i + 1];
                        }
                    }

                    return null;
                }

                int.TryParse(GetArg("-buildABVersion"), out var buildVersion);
                return buildVersion;
            }

            BuildABTool.BuildABWithVersion(buildVersion);
        }

        #endregion
    }
}