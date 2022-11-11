// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月25日, 星期一
// --------------------------

using Cysharp.Threading.Tasks;
using ET.Client;
using UnityEngine;

namespace ET
{
    public class
        GameObjectComponentSystem_AwakeSystem0 : AwakeSystem<GameObjectComponent, YooAssetProxy.YooAssetResType, string>
    {
        public override void Awake(GameObjectComponent self, YooAssetProxy.YooAssetResType a, string b)
        {

        }
    }

    public class GameObjectComponentSystem_DestroySystem0 : DestroySystem<GameObjectComponent>
    {
        public override void Destroy(GameObjectComponent self)
        {
            //self.Destroy();
        }
    }

    public static class GameObjectComponentSystem_Utilities
    {
        public static async UniTask<GameObject> CreateGameObjectInternal(this GameObjectComponent self)
        {
            GameObject gameObject =
                await GameObjectPoolComponent.Instance.FetchGameObject(self.ResName, self.YooAssetResType);
            gameObject.GetComponent<MonoBridge>().BelongToUnitId = self.GetParent<Unit>().Id;
            self.GameObject = gameObject;
            return gameObject;
        }
    }
}