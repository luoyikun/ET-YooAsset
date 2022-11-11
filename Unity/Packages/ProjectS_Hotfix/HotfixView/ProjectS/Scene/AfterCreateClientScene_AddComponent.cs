using Cysharp.Threading.Tasks;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterCreateClientScene_AddComponent: AEvent<Scene, EventType.AfterCreateClientScene>
    {
        protected override async UniTask Run(Scene scene, EventType.AfterCreateClientScene args)
        {
            scene.AddComponent<YooAssetComponent>();
            scene.AddComponent<FUIPackageManagerComponent>();
            scene.AddComponent<FUIManagerComponent>();

            await UniTask.CompletedTask;
        }
    }
}