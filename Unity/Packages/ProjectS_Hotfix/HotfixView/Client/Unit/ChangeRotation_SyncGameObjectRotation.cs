using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.SingleGame)]
    public class ChangeRotation_SyncGameObjectRotation: AEvent<Unit, EventType.ChangeRotation>
    {
        protected override async UniTask Run(Unit unit, EventType.ChangeRotation args)
        {
            GameObjectComponent gameObjectComponent = unit.GetComponent<GameObjectComponent>();
            if (gameObjectComponent == null)
            {
                return;
            }
            Transform transform = gameObjectComponent.GameObject.transform;
            transform.rotation = unit.Rotation;
            await UniTask.CompletedTask;
        }
    }
}
