// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月2日, 星期六
// --------------------------

using Bright.Serialization;
using Cysharp.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using YooAsset;

namespace ET
{
    public static class LuBanEntry
    {
        public static async UniTask<JSONNode> LoadJsonBuf(string fileName)
        {
            AssetOperationHandle result = await YooAssetProxy.LoadAssetAsync<TextAsset>(
                YooAssetProxy.GetYooAssetFormatResPath(fileName, YooAssetProxy.YooAssetResType.Config));
            return JSON.Parse(result.GetAssetObject<TextAsset>().text);
        }

        public static async UniTask<ByteBuf> LoadBytesBuf(string fileName)
        {
            AssetOperationHandle result = await YooAssetProxy.LoadAssetAsync<TextAsset>(
                YooAssetProxy.GetYooAssetFormatResPath(fileName, YooAssetProxy.YooAssetResType.Config));
            return new ByteBuf(result.GetAssetObject<TextAsset>().bytes);
        }
    }
}