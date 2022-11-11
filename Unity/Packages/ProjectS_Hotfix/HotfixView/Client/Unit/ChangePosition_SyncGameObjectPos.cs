using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.SingleGame)]
    public class ChangePosition_SyncGameObjectPos: AEvent<Unit, EventType.ChangePosition>
    {
        protected override async UniTask Run(Unit unit, EventType.ChangePosition args)
        {
            GameObjectComponent gameObjectComponent = unit.GetComponent<GameObjectComponent>();
            if (gameObjectComponent == null)
            {
                return;
            }
            Transform transform = gameObjectComponent.GameObject.transform;
            transform.position = unit.Position;
            await UniTask.CompletedTask;
        }
    }
}