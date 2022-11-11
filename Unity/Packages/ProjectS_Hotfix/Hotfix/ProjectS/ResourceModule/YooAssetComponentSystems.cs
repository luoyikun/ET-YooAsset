// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月13日, 星期三
// --------------------------

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ET
{
    public class YooAssetComponentDestroySystem : DestroySystem<YooAssetComponent>
    {
        public override void Destroy(YooAssetComponent self)
        {
            foreach (var assetsOperation in self.AssetsOperationHandles)
            {
                assetsOperation.Value.Release();
            }

            foreach (var assetsOperation in self.SubAssetsOperationHandles)
            {
                assetsOperation.Value.Release();
            }

            foreach (var sceneOperationHandle in self.SceneOperationHandles)
            {
                sceneOperationHandle.Value.UnloadAsync();
            }

            self.SceneOperationHandles.Clear();
            self.RawFileOperationHandles.Clear();
            self.AssetsOperationHandles.Clear();
            self.SubAssetsOperationHandles.Clear();
        }
    }

    public static class YooAssetComponentSystems
    {
        public static async UniTask<T> LoadAssetAsync<T>(this YooAssetComponent self, string path)
            where T : UnityEngine.Object
        {
            // 异步重入，避免重复加载导致的handle泄露
            var cachedResult = GetRecord(self.AssetsOperationHandles, path);
            if (cachedResult != null)
            {
                return cachedResult.GetAssetObject<T>();
            }

            AssetOperationHandle assetOperationHandle = await YooAssetProxy.LoadAssetAsync<T>(path);

            // 异步重入，避免重读加载导致的Handle泄露
            var finalResult = GetOrRecordToMap(self.AssetsOperationHandles, path, assetOperationHandle);

            if (finalResult != assetOperationHandle)
            {
                assetOperationHandle.Release();
            }
            
            return finalResult.GetAssetObject<T>();
        }

        public static async UniTask<T> LoadSubAssetsAsync<T>(this YooAssetComponent self,
            string mainAssetPath, string subAssetName)
            where T : UnityEngine.Object
        {
            // 异步重入，避免重复加载导致的handle泄露
            var cachedResult = GetRecord(self.SubAssetsOperationHandles, mainAssetPath);
            if (cachedResult != null)
            {
                return cachedResult.GetSubAssetObject<T>(subAssetName);
            }
            
            SubAssetsOperationHandle subAssetsOperationHandle =
                await YooAssetProxy.LoadSubAssetsAsync<T>(mainAssetPath, subAssetName);

            // 异步重入，避免重读加载导致的Handle泄露
            var finalResult = GetOrRecordToMap(self.SubAssetsOperationHandles, mainAssetPath, subAssetsOperationHandle);
            
            if (finalResult != subAssetsOperationHandle)
            {
                subAssetsOperationHandle.Release();
            }
            
            return finalResult.GetSubAssetObject<T>(subAssetName);
        }

        public static async UniTask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(this YooAssetComponent self,
            string scenePath,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            // 异步重入，避免重复加载导致的handle泄露
            var cachedResult = GetRecord(self.SceneOperationHandles, scenePath);
            if (cachedResult != null)
            {
                return cachedResult.SceneObject;
            }
            
            SceneOperationHandle sceneOperationHandle = await YooAssetProxy.LoadSceneAsync(scenePath, loadSceneMode);

            // 异步重入，避免重读加载导致的Handle泄露
            var finalResult = GetOrRecordToMap(self.SceneOperationHandles, scenePath, sceneOperationHandle);
            return finalResult.SceneObject;
        }

        public static async UniTask<string> GetRawFileAsync_Txt(this YooAssetComponent self, string path)
        {
            // 异步重入，避免重复加载导致的handle泄露
            if (self.RawFileOperationHandles.TryGetValue(path, out var cechedResult))
            {
                return cechedResult.GetRawString();
            }

            RawFileOperation rawFileOperation = await YooAssetProxy.GetRawFileAsync(path);

            if (self.RawFileOperationHandles.TryGetValue(path, out var result))
            {
                return result.GetRawString();
            }
            else
            {
                self.RawFileOperationHandles[path] = rawFileOperation;
            }

            return rawFileOperation.GetRawString();
        }

        public static async UniTask<byte[]> GetRawFileAsync_Bytes(this YooAssetComponent self, string path)
        {
            // 异步重入，避免重复加载导致的handle泄露
            if (self.RawFileOperationHandles.TryGetValue(path, out var cechedResult))
            {
                return cechedResult.GetRawBytes();
            }

            RawFileOperation rawFileOperation = await YooAssetProxy.GetRawFileAsync(path);

            if (self.RawFileOperationHandles.TryGetValue(path, out var result))
            {
                return result.GetRawBytes();
            }
            else
            {
                self.RawFileOperationHandles[path] = rawFileOperation;
            }

            return rawFileOperation.GetRawBytes();
        }

        private static T GetRecord<T>(Dictionary<string, T> map, string path) where T : OperationHandleBase
        {
            if (map.TryGetValue(path, out var result))
            {
                return result;
            }
            else
            {
                return default;
            }
        }
        
        private static T GetOrRecordToMap<T>(Dictionary<string, T> map, string path,
            OperationHandleBase operationHandleBase) where T : OperationHandleBase
        {
            // 异步重入，避免重复加载导致的handle泄露
            if (map.TryGetValue(path, out var conflictedAssetOperationHandle))
            {
                return conflictedAssetOperationHandle;
            }
            else
            {
                map[path] = operationHandleBase as T;
            }

            return operationHandleBase as T;
        }
    }
}