// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月16日, 星期六
// --------------------------

using Cysharp.Threading.Tasks;
using ET.EventType;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class FinishLoad_HideLoadingUI : AEvent<Scene, LoadingFinish>
    {
        protected override async UniTask Run(Scene entity, LoadingFinish a)
        {
            var scene = entity;

            FUIManagerComponent fuiManagerComponent = scene.GetComponent<FUIManagerComponent>();

            FUI_LoadingComponent fuiLoadingComponent =
                fuiManagerComponent.GetFUIComponent<FUI_LoadingComponent>(FUIPackage.Loading);

            fuiLoadingComponent.Reset();
            fuiLoadingComponent.Hide();
        }
    }
}