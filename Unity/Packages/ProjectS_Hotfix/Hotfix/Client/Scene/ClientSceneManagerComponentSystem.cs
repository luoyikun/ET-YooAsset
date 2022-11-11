using System;

namespace ET.Client
{
    [FriendOf(typeof(ClientSceneManagerComponent))]
    public static class ClientSceneManagerComponentSystem
    {
        [ObjectSystem]
        public class ClientSceneManagerComponentAwakeSystem : AwakeSystem<ClientSceneManagerComponent>
        {
            public override void Awake(ClientSceneManagerComponent self)
            {
                ClientSceneManagerComponent.Instance = self;
            }
        }

        [ObjectSystem]
        public class ClientSceneManagerComponentDestroySystem : DestroySystem<ClientSceneManagerComponent>
        {
            public override void Destroy(ClientSceneManagerComponent self)
            {
                self.ClientScenes.Clear();
            }
        }

        public static Scene ClientScene(this Entity entity)
        {
            return ClientSceneManagerComponent.Instance.Get(entity.DomainZone());
        }

        public static void Add(this ClientSceneManagerComponent self, Scene clientScene)
        {
            self.ClientScenes.Add(clientScene.Zone, clientScene);
        }

        public static Scene Get(this ClientSceneManagerComponent self, int zone)
        {
            self.ClientScenes.TryGetValue(zone, out Scene scene);
            return scene;
        }

        public static void Remove(this ClientSceneManagerComponent self, int zone)
        {
            self.ClientScenes.Remove(zone);
        }

        /// <summary>
        /// Warning 不应当被频繁调用，而是通过Component的Domain来获取
        /// 只有进行Scene切换，销毁的时候才调用
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Scene SetCurrentSingleGameScene(this ClientSceneManagerComponent self, Scene newSingleGameScene)
        {
            if (self.CurrentSingleGameSceneZone != 0 && self.GetCurrentSingleGameScene() != null)
            {
                self.GetCurrentSingleGameScene().Dispose();
            }

            self.CurrentSingleGameSceneZone = newSingleGameScene.Zone;
            return default;
        }

        /// <summary>
        /// Warning 不应当被频繁调用，而是通过Component的Domain来获取
        /// 只有进行Scene切换，销毁的时候才调用
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Scene GetCurrentSingleGameScene(this ClientSceneManagerComponent self)
        {
            if (self.CurrentSingleGameSceneZone == 0)
            {
                Log.Error("当前未设置SingleGameScene");
                return default;
            }

            if (self.ClientScenes.TryGetValue(self.CurrentSingleGameSceneZone, out var result))
            {
                return result;
            }

            Log.Error($"未找到Zone为{self.CurrentSingleGameSceneZone}的SingleGameScene，请检查逻辑");
            return default;
        }
    }
}