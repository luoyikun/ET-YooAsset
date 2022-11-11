using UnityEngine;

namespace ET
{
    namespace EventType
    {
        public struct ChangePosition
        {
            public Vector3 OldPos;
        }

        public struct ChangeRotation
        {
        }

        public struct CreateColliderGameObject
        {
            public YooAssetProxy.YooAssetResType Type;
            public UnitDefine.CreateColliderArgs CreateColliderArgs;
        }
    }
}