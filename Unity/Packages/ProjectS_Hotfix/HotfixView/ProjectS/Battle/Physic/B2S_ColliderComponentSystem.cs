// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月26日, 星期二
// --------------------------

using Cysharp.Threading.Tasks;
using ET.Client;
using ET.EventType;
using UnityEngine;

namespace ET
{
    [Callback(CallbackType.GetMonoBridge)]
    public class B2S_ColliderComponent_GetMonoBridge : IFunc<B2S_ColliderComponent, MonoBridge>
    {
        public MonoBridge Handle(B2S_ColliderComponent a)
        {
            return a.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject.GetComponent<MonoBridge>();
        }
    }

    [Event(SceneType.SingleGame)]
    public class B2S_ColliderComponent_CreateGo : AEvent<Unit, EventType.CreateColliderGameObject>
    {
        protected override async UniTask Run(Unit entity, CreateColliderGameObject a)
        {
            GameObject go = await entity
                .AddComponent<GameObjectComponent, YooAssetProxy.YooAssetResType, string>(
                    YooAssetProxy.YooAssetResType.Effect, a.CreateColliderArgs.PrefabABPath).CreateGameObjectInternal();

            go.transform.position = a.CreateColliderArgs.BelontToUnit.Position;
            go.transform.rotation = a.CreateColliderArgs.BelontToUnit.Rotation;
        }
    }

    public static class B2S_ColliderComponent_Utilities
    {
    }
}