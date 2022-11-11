// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月11日, 星期一
// --------------------------

using Cysharp.Threading.Tasks;
using ET.EventType;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class BeginLoad_ShowLoadingUI : AEvent<Scene, LoadingBegin>
    {
        protected override async UniTask Run(Scene entity, LoadingBegin a)
        {
            var scene = entity;

            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();

            FUI_LoadingComponent fuiLoadingComponent =
                fuiManagerComponent.GetFUIComponent<FUI_LoadingComponent>(FUIPackage.Loading);
            
            if (fuiLoadingComponent == null)
            {
                // 别忘了自己处理依赖关系包
                await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.Shared);
                await scene.GetComponent<FUIPackageManagerComponent>().AddPackageAsync(FUIPackage.Loading);

                FUI_Loading fuiLoading = await FUI_Loading.CreateInstanceAsync(scene);

                fuiLoadingComponent = fuiLoading.AddChild<FUI_LoadingComponent, FUI_Loading>(fuiLoading);

                fuiManagerComponent.Add(FUIPackage.Loading, fuiLoading, fuiLoadingComponent);
            }

            fuiLoadingComponent.Reset();
            fuiLoadingComponent.Show();

            await fuiLoadingComponent.LoadRes(a.SceneName, a.ResList);
        }
    }
}