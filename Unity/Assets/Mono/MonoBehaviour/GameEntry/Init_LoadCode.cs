// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月10日, 星期日
// --------------------------

using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using YooAsset;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HybridCLR;

namespace ET
{
    public partial class Init
    {
        // 加载热更层代码
        private async UniTask LoadCode()
        {
            byte[] config = (await YooAssetProxy.GetRawFileAsync("Config_DLLNameListForAOT")).LoadFileData();
            DLLNameListForAOT dllNameListForAOT =
                SerializationUtility.DeserializeValue<DLLNameListForAOT>(config, DataFormat.JSON);

            List<UniTask<RawFileOperation>> tasks = new List<UniTask<RawFileOperation>>();

            foreach (var aotDll in dllNameListForAOT.DLLNameList_ForABLoad)
            {
                Debug.Log($"添加{aotDll}");
                tasks.Add(YooAssetProxy.GetRawFileAsync(aotDll));
            }

            RawFileOperation[] rawFileOperations = await UniTask.WhenAll(tasks);

            foreach (var task in rawFileOperations)
            {
                Debug.Log("准备加载AOT补充元数据");
                LoadMetadataForAOTAssembly(task.GetRawBytes());
            }

            await CodeLoader.Instance.Start();
            Log.Info("Dll加载完毕，正式进入游戏流程");

            static unsafe void LoadMetadataForAOTAssembly(byte[] dllBytes)
            {
                fixed (byte* ptr = dllBytes)
                {
#if !UNITY_EDITOR
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                int err = RuntimeApi.LoadMetadataForAOTAssembly((IntPtr)ptr, dllBytes.Length);
                Debug.Log("LoadMetadataForAOTAssembly. ret:" + err);
#endif
                }
            }
        }
    }
}