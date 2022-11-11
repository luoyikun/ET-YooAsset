using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Process)]
    public class AppStart_Init: AEvent<Scene, EventType.AppStart>
    {
        protected override async UniTask Run(Scene scene, EventType.AppStart args)
        {
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();
            
            Game.Scene.AddComponent<ConfigComponent>();
            await ConfigComponent.Instance.LoadAsync();

            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            
            Game.Scene.AddComponent<ClientSceneManagerComponent>();
            
            Game.Scene.AddComponent<GlobalComponent>();
            Game.Scene.AddComponent<NumericWatcherComponent>();

            // 创建整个游戏级的Scene
            Scene clientScene = Client.SceneFactory.CreateClientScene(1, "Game", Game.Scene);
            
            await Game.EventSystem.PublishAsync(clientScene, new EventType.AppStartInitFinish());
        }
    }
}
