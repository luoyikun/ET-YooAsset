// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月13日, 星期三
// --------------------------

using Cysharp.Threading.Tasks;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class AfterCreateSingleGameScene_AddComponent : AEvent<Scene, EventType.AfterCreateSingleGameScene_Logic>
    {
        protected override async UniTask Run(Scene scene, EventType.AfterCreateSingleGameScene_Logic args)
        {
            scene.AddComponent<GameObjectPoolComponent>();
            scene.AddComponent<YooAssetComponent>();
            scene.AddComponent<FUIPackageManagerComponent>();
            scene.AddComponent<FUIManagerComponent>();
            scene.AddComponent<PathFindComponent>();
            scene.AddComponent<UserInputComponent>();

            await UniTask.CompletedTask;
        }
    }
}