using System;
using UnityEngine;

namespace ET.Client
{
    public static class GameObjectComponentSystem
    {
        [ObjectSystem]
        public class DestroySystem : DestroySystem<GameObjectComponent>
        {
            public override void Destroy(GameObjectComponent self)
            {
                GameObjectPoolComponent.Instance.RecycleGameObject(self.ResName, self.GameObject);
            }
        }

        [ObjectSystem]
        public class
            DestroySystemAwakeComponent : AwakeSystem<GameObjectComponent, YooAssetProxy.YooAssetResType, string>
        {
            public override void Awake(GameObjectComponent self, YooAssetProxy.YooAssetResType yooAssetResType,
                string resName)
            {
                self.YooAssetResType = yooAssetResType;
                self.ResName = resName;
            }
        }
    }
}