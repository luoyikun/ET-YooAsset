// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月25日, 星期一
// --------------------------

using UnityEngine;

namespace ET
{
    public class UnitDefine
    {
        public class CreateColliderArgs : IReference
        {
            public string PrefabABPath;

            public int NP_TreeConfigId;

            public Unit BelontToUnit;

            /// <summary>
            /// 将要发生碰撞事件的Tag
            /// </summary>
            public RoleTag TargetCollsionRoleTag;

            /// <summary>
            /// 将要发生碰撞事件的Cast
            /// </summary>
            public RoleCast TargetCollsionRoleCast;

            /// <summary>
            /// 将要发生碰撞事件的Camp
            /// </summary>
            public RoleCamp TargetCollsionRoleCamp;

            public bool FollowUnit;

            public Vector3 Offset;

            public Vector3 TargetPos;

            public float Angle;

            /// <summary>
            /// 碰撞开始时黑板键
            /// </summary>
            public string OnTriggerEnter;

            /// <summary>
            /// 碰撞持续时黑板键
            /// </summary>
            public string OnTriggerStay;

            /// <summary>
            /// 碰撞结束时黑板键
            /// </summary>
            public string OnTriggerExit;

            public CreateColliderArgs Init(string prefabABPath, int npTreeConfigId, Unit belongToUnit, float angle,
                bool followUnit,
                RoleCast targetCollsionRoleCast, RoleCamp targetCollsionRoleCamp, RoleTag targetCollsionRoleTag,
                Vector3 offset, Vector3 targetPos, string onTriggerEnter, string onTriggerStay, string onTriggerExit)
            {
                this.PrefabABPath = prefabABPath;
                this.NP_TreeConfigId = npTreeConfigId;
                BelontToUnit = belongToUnit;
                Angle = angle;
                FollowUnit = followUnit;
                TargetCollsionRoleCamp = targetCollsionRoleCamp;
                TargetCollsionRoleTag = targetCollsionRoleTag;
                TargetCollsionRoleCast = targetCollsionRoleCast;
                Offset = offset;
                TargetPos = targetPos;
                this.OnTriggerEnter = onTriggerEnter;
                this.OnTriggerStay = onTriggerStay;
                this.OnTriggerExit = onTriggerExit;
                return this;
            }

            public void Clear()
            {
                BelontToUnit = null;
                TargetCollsionRoleCamp = RoleCamp.Player;
                TargetCollsionRoleTag = RoleTag.Hero;
                TargetCollsionRoleCast = RoleCast.Friendly;
                FollowUnit = false;
                Offset = Vector3.zero;
                TargetPos = Vector3.zero;

                Angle = 0;
                OnTriggerEnter = string.Empty;
                OnTriggerStay = string.Empty;
                OnTriggerExit = string.Empty;
            }
        }
    }
}