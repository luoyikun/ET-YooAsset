// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月25日, 星期一
// --------------------------

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ET.Client;
using UnityEngine;

namespace ET
{
    public class GameObjectPoolComponent_AwakeSystem0 : AwakeSystem<GameObjectPoolComponent>
    {
        public override void Awake(GameObjectPoolComponent self)
        {
            GameObjectPoolComponent.Instance = self;
        }
    }

    public class GameObjectPoolComponent_DestroySystem0 : DestroySystem<GameObjectPoolComponent>
    {
        public override void Destroy(GameObjectPoolComponent self)
        {
        }
    }


    public static class GameObjectPoolComponent_Utilities
    {
        public static async UniTask<GameObject> FetchGameObject(this GameObjectPoolComponent self, string resName,
            YooAssetProxy.YooAssetResType gameObjectType)
        {
            GameObject gameObject;
            if (self.AllCachedGos.TryGetValue(resName, out var gameObjects))
            {
                if (gameObjects.Count > 0)
                {
                    gameObject = gameObjects.Dequeue();
                }
                else
                {
                    gameObject =
                        UnityEngine.Object.Instantiate(self.AllPrefabs[resName], GlobalComponent.Instance.Unit, true);
                }
            }
            else
            {
                GameObject targetprefab;
                if (self.AllPrefabs.TryGetValue(resName, out var prefab))
                {
                    targetprefab = prefab;
                }
                else
                {
                    YooAssetComponent yooAssetComponent = self.DomainScene().GetComponent<YooAssetComponent>();

                    targetprefab = await yooAssetComponent.LoadAssetAsync<GameObject>(
                        YooAssetProxy.GetYooAssetFormatResPath(resName, gameObjectType));

                    if (targetprefab == null)
                    {
                        return null;
                    }

                    self.AllPrefabs[resName] = targetprefab;
                    self.AllCachedGos[resName] = new Queue<GameObject>();
                }

                gameObject = UnityEngine.Object.Instantiate(targetprefab, GlobalComponent.Instance.Unit, true);
                gameObject.transform.position = targetprefab.transform.position;
                gameObject.name = "Need To Be Renamed";
            }

            gameObject.SetActive(true);
            return gameObject;
        }

        public static void RecycleGameObject(this GameObjectPoolComponent self, string resName, GameObject gameObject)
        {
            gameObject.SetActive(false);
            if (string.IsNullOrEmpty(resName))
            {
                return;
            }

            self.AllCachedGos[resName].Enqueue(gameObject);
        }
    }
}