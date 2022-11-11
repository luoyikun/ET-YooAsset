// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月27日, 星期三
// --------------------------

using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.SceneManagement;
using YooAsset.Editor;

namespace ET
{
    /// <summary>
    /// 编辑器下一键Build AB
    /// </summary>
    public static class BuildABTool
    {
        [MonKey.Command("Build AB", "ProjectS编辑器下Build AB工具", Category = "Build")]
        public static void BuildAB_Auto()
        {
            int cachedVersion = EditorPrefs.GetInt("YooAssetABVersion");
            cachedVersion++;
            EditorPrefs.SetInt("YooAssetABVersion", cachedVersion);
            
            BuildABWithVersion(cachedVersion);
        }

        public static void BuildABWithVersion(int buildVersion = 0)
        {
            Debug.Log($"开始构建AB，平台为：{EditorUserBuildSettings.activeBuildTarget} Version为 {buildVersion}");

            EditorSceneManager.OpenScene(GlobalDefine.InitScenePath);

            // 构建参数
            string defaultOutputRoot = AssetBundleBuilderHelper.GetDefaultOutputRoot();
            BuildParameters buildParameters = new BuildParameters();
            buildParameters.OutputRoot = defaultOutputRoot;
            buildParameters.BuildTarget = EditorUserBuildSettings.activeBuildTarget;
            buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline;
            buildParameters.BuildMode = EBuildMode.IncrementalBuild;
            buildParameters.BuildVersion = buildVersion;
            buildParameters.BuildinTags = "buildin";
            buildParameters.VerifyBuildingResult = true;
            buildParameters.EnableAddressable = true;
            buildParameters.CopyBuildinTagFiles = true;
            buildParameters.EncryptionServices = new GameEncryption();
            buildParameters.CompressOption = ECompressOption.LZ4;

            // 执行构建
            AssetBundleBuilder builder = new AssetBundleBuilder();
            bool succeed = builder.Run(buildParameters);

            // 需要对构建出的AB包进行后处理
            // 1. 更改文件夹名为App version名
            // 2. 将文件夹移动到同父级目录的CDN文件夹下
            string oriABPath =
                $"{buildParameters.OutputRoot}/{EditorUserBuildSettings.activeBuildTarget}/{buildVersion}";
            string finalABDir =
                $"{buildParameters.OutputRoot}/CDN/{EditorUserBuildSettings.activeBuildTarget}";
            string finalABPath =
                $"{buildParameters.OutputRoot}/CDN/{EditorUserBuildSettings.activeBuildTarget}/{Init.Instance.Version}";
            if (Directory.Exists(finalABDir))
            {
                Directory.Delete(finalABDir, true);
            }

            Directory.CreateDirectory(finalABDir);
            Directory.Move(oriABPath, finalABPath);

            Debug.Log($"构建AB结果:{succeed}，将{oriABPath}移动到{finalABPath}");
        }
    }
}