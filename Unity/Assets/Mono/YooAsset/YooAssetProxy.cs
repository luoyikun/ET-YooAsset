using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using YooAsset;
using UnityEngine;

namespace ET
{
    public static class YooAssetProxy
    {
        public enum YooAssetResType
        {
            Unit,
            Config,
            DataConfig,
            SkillConfig,
            Scene,
            Code,
            Effect,
            Shader,
            Material,
            Sound,
            PathFind,
        }
        
        #region 路径相关

        /// <summary>
        /// 获取YooAsset能识别的资源名
        /// </summary>
        /// <returns></returns>
        public static string GetYooAssetFormatResPath(string rawResName, YooAssetResType yooAssetResType)
        {
            return $"{yooAssetResType}_{rawResName}";
        }

        #endregion
        
        #region Extension

        public static T GetAsset<T>(this AssetOperationHandle assetOperationHandle)
            where T : UnityEngine.Object
        {
            return assetOperationHandle.GetAssetObject<T>();
        }

        public static T GetSubAsset<T>(this SubAssetsOperationHandle assetOperationHandle, string subAssetName)
            where T : UnityEngine.Object
        {
            return assetOperationHandle.GetSubAssetObject<T>(subAssetName);
        }

        public static byte[] GetRawBytes(this RawFileOperation rawFileOperation)
        {
            return rawFileOperation.LoadFileData();
        }

        public static string GetRawString(this RawFileOperation rawFileOperation)
        {
            return rawFileOperation.LoadFileText();
        }

        #endregion

        #region API

        public static async UniTask<AssetOperationHandle> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            AssetOperationHandle assetOperationHandle = YooAssets.LoadAssetAsync<T>(path);
            await assetOperationHandle.ToUniTask();
            return assetOperationHandle;
        }

        public static async UniTask<SubAssetsOperationHandle> LoadSubAssetsAsync<T>(string mainAssetPath, string subAssetName)
            where T : UnityEngine.Object
        {
            SubAssetsOperationHandle subAssetsOperationHandle = YooAssets.LoadSubAssetsAsync<T>(mainAssetPath);
            await subAssetsOperationHandle.ToUniTask();
            return subAssetsOperationHandle;
        }

        public static async UniTask<SceneOperationHandle> LoadSceneAsync(string scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneOperationHandle sceneOperationHandle = YooAssets.LoadSceneAsync(scenePath, loadSceneMode, true);
            await sceneOperationHandle.ToUniTask();
            return sceneOperationHandle;
        }

        public static async UniTask<RawFileOperation> GetRawFileAsync(string path)
        {
            RawFileOperation rawFileOperation = YooAssets.GetRawFileAsync(path);
            await rawFileOperation.ToUniTask();
            return rawFileOperation;
        }

        public static void UnloadUnusedAssets()
        {
            YooAssets.UnloadUnusedAssets();
        }

        public static List<string> GetAssetPathsByTag(string tag)
        {
            AssetInfo[] assetInfos = YooAssets.GetAssetInfos(tag);
            List<string> result = new List<string>(16);
            foreach (var assetInfo in assetInfos)
            {
                result.Add(assetInfo.Address);
            }

            return result;
        }

        public static void InitHostCallbacks(Action<PatchEventMessageDefine.PatchStatesChange> onStateUpdate,
            Action<PatchEventMessageDefine.DownloadProgressUpdate> onDownLoadProgressUpdate)
        {
            PatchUpdater.InitCallback(onStateUpdate, onDownLoadProgressUpdate);
        }

        public static UniTask StartYooAssetEngine(YooAssets.EPlayMode playMode)
        {
            UniTaskCompletionSource uniTaskCompletionSource = new UniTaskCompletionSource();

            // 编辑器下的模拟模式
            if (playMode == YooAssets.EPlayMode.EditorSimulateMode)
            {
                var createParameters = new YooAssets.EditorSimulateModeParameters();
                createParameters.LocationServices = new AddressLocationServices();
                YooAssets.InitializeAsync(createParameters).Completed += _ => { uniTaskCompletionSource.TrySetResult(); };
            }

            // 单机运行模式
            if (playMode == YooAssets.EPlayMode.OfflinePlayMode)
            {
                var createParameters = new YooAssets.OfflinePlayModeParameters();
                createParameters.LocationServices = new AddressLocationServices();
                YooAssets.InitializeAsync(createParameters).Completed += _ => { uniTaskCompletionSource.TrySetResult(); };
            }

            // 联机运行模式
            if (playMode == YooAssets.EPlayMode.HostPlayMode)
            {
                var createParameters = new YooAssets.HostPlayModeParameters();
                createParameters.LocationServices = new AddressLocationServices();
                createParameters.DecryptionServices = null;
                createParameters.ClearCacheWhenDirty = false;
                createParameters.DefaultHostServer = GetHostServerURL();
                createParameters.FallbackHostServer = GetHostServerURL();
                
                string GetHostServerURL()
                {
                    string hostServerIP = Init.Instance.HotfixResUrl;
                    string gameVersion = Init.Instance.Version;
                    
#if UNITY_EDITOR
                    if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                        return $"{hostServerIP}/CDN/Android/{gameVersion}";
                    else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                        return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
                    else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                        return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
                    else
                        return $"{hostServerIP}/CDN/StandaloneWindows64/{gameVersion}";
#else
		            if (Application.platform == RuntimePlatform.Android)
			            return $"{hostServerIP}/CDN/Android/{gameVersion}";
		            else if (Application.platform == RuntimePlatform.IPhonePlayer)
			            return $"{hostServerIP}/CDN/IPhone/{gameVersion}";
		            else if (Application.platform == RuntimePlatform.WebGLPlayer)
			            return $"{hostServerIP}/CDN/WebGL/{gameVersion}";
		            else
			            return $"{hostServerIP}/CDN/StandaloneWindows64/{gameVersion}";
#endif
                    return $"{hostServerIP}/CDN/StandaloneWindows64/{gameVersion}";
                }

                // 如果是资源热更模式，则需要等待热更完毕后再Invoke回调
                YooAssets.InitializeAsync(createParameters).Completed += _ =>
                {
                    // 运行补丁流程
                    PatchUpdater.Run(uniTaskCompletionSource);
                };
            }

            return uniTaskCompletionSource.Task;
        }
        
        

        #endregion
    }
}