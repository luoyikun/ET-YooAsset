using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Profiling;
using YooAsset;
using Debug = UnityEngine.Debug;

namespace ET
{
#if UNITY_EDITOR
    // 注册Editor下的Log
    [InitializeOnLoad]
    public class EditorRegisteLog
    {
        static EditorRegisteLog()
        {
            Game.ILog = new UnityLogger();
        }
    }
#endif

    public partial class Init : MonoSingleton<Init>
    {
        [LabelText("资源服地址")] public string HotfixResUrl = "http://127.0.0.1:8088";

        [InfoBox("例如v1.0")]
        [LabelText("版本标识")] public string Version = "v0.0.1";

        [LabelText("资源模式")]
        public YooAssets.EPlayMode PlayMode = YooAssets.EPlayMode.EditorSimulateMode;

        private void Awake()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };

            SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

            DontDestroyOnLoad(gameObject);

            Game.ILog = new UnityLogger();

            FUIEntry.Init();

            if (PlayMode == YooAssets.EPlayMode.HostPlayMode)
            {
                FUI_CheckForResUpdateComponent.Init();
            }

            LoadAssetsAndHotfix().Forget();
        }

        private async UniTaskVoid LoadAssetsAndHotfix()
        {
            // 启动YooAsset引擎，并在初始化完毕后进行热更代码加载
            await YooAssetProxy.StartYooAssetEngine(PlayMode);

            // Shader Warm Up
            ShaderVariantCollection shaderVariantCollection =
                (await YooAssetProxy.LoadAssetAsync<ShaderVariantCollection>(
                    YooAssetProxy.GetYooAssetFormatResPath("ProjectSShaderVariant",
                        YooAssetProxy.YooAssetResType.Shader)))
                .GetAssetObject<ShaderVariantCollection>();

            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.Info(
                $"开始Shader Warm Up, shaderCount: {shaderVariantCollection.shaderCount} variantCount: {shaderVariantCollection.variantCount}");

            shaderVariantCollection.WarmUp();

            stopwatch.Stop();

            Log.Info($"Shader Warm Up完成, 耗时: {stopwatch.ElapsedMilliseconds}ms");

            await LoadCode();

            if (PlayMode == YooAssets.EPlayMode.HostPlayMode)
            {
                FUI_CheckForResUpdateComponent.Release();
            }
        }

        private void Update()
        {
            CodeLoader.Instance.Update?.Invoke();
        }

        private void FixedUpdate()
        {
            CodeLoader.Instance.FixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            CodeLoader.Instance.LateUpdate?.Invoke();
        }

        private void OnApplicationQuit()
        {
            CodeLoader.Instance.OnApplicationQuit();
            CodeLoader.Instance.Dispose();
        }
    }
}