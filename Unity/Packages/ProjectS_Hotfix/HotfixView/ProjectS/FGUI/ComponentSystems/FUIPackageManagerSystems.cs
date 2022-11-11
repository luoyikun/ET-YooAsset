// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月13日, 星期三
// --------------------------

using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace ET
{
    public class FUIPackageManagerSystems : AwakeSystem<FUIPackageManagerComponent>
    {
        public override void Awake(FUIPackageManagerComponent self)
        {
            self.UsedYooAssetComponent = self.Domain.GetComponent<YooAssetComponent>();
        }
    }

    public class FUIPackageManagerDestroySystems : DestroySystem<FUIPackageManagerComponent>
    {
        public override void Destroy(FUIPackageManagerComponent self)
        {
            self.UsedYooAssetComponent = null;
        }
    }

    public static class FUIPackageManagerUtitlits
    {
        public static async UniTask AddPackageAsync(this FUIPackageManagerComponent self, string type)
        {
            // 先加载UI描述文件
            TextAsset desTextAsset = await self.UsedYooAssetComponent.LoadAssetAsync<TextAsset>($"FGUI_{type}_fui");

            // 再加载UI图集文件
            UIPackage.AddPackage(desTextAsset.bytes, type, self.LoadPackageInternalAsync);
        }

        /// <summary>
        /// 加载资源的异步委托
        /// </summary>
        /// <param name="name">注意，这个name是FGUI内部组装的纹理全名，例如FUILogin_atlas0</param>
        /// <param name="extension"></param>
        /// <param name="type"></param>
        /// <param name="item"></param>
        private static async void LoadPackageInternalAsync(this FUIPackageManagerComponent self, string name,
            string extension, System.Type type, PackageItem item)
        {
            item.owner.SetItemAsset(item, await self.UsedYooAssetComponent.LoadAssetAsync<Texture>($"FGUI_{name}"),
                DestroyMethod.Unload);
        }
    }
}