//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月29日 18:21:36
//------------------------------------------------------------

using Cysharp.Threading.Tasks;
using ET;
using FairyGUI;
using UnityEngine;
using YooAsset;

namespace ET
{
    /// <summary>
    /// 使用时对GLoader的Url赋值为目标纹理asset的ab包全路径即可
    /// </summary>
    public class NKGGLoader : GLoader
    {
        private AssetOperationHandle _assetOperationHandle;
        
        protected override void LoadExternal()
        {
            LoadSprite().Forget();
        }

        private async UniTaskVoid LoadSprite()
        {
            _assetOperationHandle = await YooAssetProxy.LoadAssetAsync<Sprite>(this.url);
            Sprite sprite =_assetOperationHandle.GetAssetObject<Sprite>();
            if (sprite != null)
                onExternalLoadSuccess(new NTexture(sprite));
            else
                onExternalLoadFailed();
        }

        protected override void FreeExternal(NTexture texture)
        {
            //释放外部载入的资源
            _assetOperationHandle.Release();
        }
    }
}