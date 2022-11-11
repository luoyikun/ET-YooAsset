//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月27日 22:26:39
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ET.Client;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 创建碰撞体Action
    /// 创建碰撞体会创建一个UnitA作为碰撞体的载体，然后为其添加碰撞相关组件让其变成一个游戏中的碰撞体
    /// 碰撞体一般都会有一个BelongToUnit，指向释放者
    /// Box2D碰撞体每帧都会同步UnitA的Transform信息作为自己在Box2D世界中的Transform信息
    /// 所以FollowUnit选项只是让我们决定UnitA的Transform是否每帧由BelongToUnit决定
    /// </summary>
    [Title("创建一个碰撞体", TitleAlignment = TitleAlignments.Centered)]
    public class NP_CreateColliderAction : NP_ClassForStoreAction
    {
        [LabelText("将要实例化的Prefab")] public string PrefabABPath;

        [LabelText("将要挂载的行为树Id")] public int NP_TreeConfigId;

        [LabelText("将要碰撞的Cast")] public RoleCast TargetCollisionRoleCast;

        [LabelText("将要碰撞的Camp")] public RoleCamp TargetCollisionRoleCamp;

        [LabelText("将要碰撞的Tag")] public RoleTag TargetCollisionRoleTag;

        /// <summary>
        /// 比如诺克释放了Q技能，这里如果为True，Q技能的碰撞体就会跟随诺克
        /// </summary>
        [LabelText("是否跟随释放的Unit")] public bool FollowUnit;

        /// <summary>
        /// 只在跟随Unit时有效，因为不跟随Unit说明是世界空间的碰撞体，
        /// </summary>
        [ShowIf(nameof(FollowUnit))] [LabelText("相对于释放者的偏移量")]
        public Vector3 Offset;

        /// <summary>
        /// 只在不跟随Unit时有效，跟随Unit代表使用BelongToUnit的Transform
        /// </summary>
        [HideIf(nameof(FollowUnit))] [LabelText("旋转角度")]
        public NP_BlackBoardRelationData Angle = new NP_BlackBoardRelationData();

        /// <summary>
        /// 只在不跟随Unit时有效，因为不跟随Unit说明是世界空间的碰撞体，
        /// </summary>
        [HideIf(nameof(FollowUnit))] [LabelText("目标位置")]
        public Vector3 TargetPos;
        
        /// <summary>
        /// 碰撞开始时黑板键
        /// </summary>
        [LabelText("碰撞开始时黑板键")]
        public string OnTriggerEnter;

        /// <summary>
        /// 碰撞持续时黑板键
        /// </summary>
        [LabelText("碰撞持续时黑板键")]
        public string OnTriggerStay;

        /// <summary>
        /// 碰撞结束时黑板键
        /// </summary>
        [LabelText("碰撞结束时黑板键")]
        public string OnTriggerExit;

        public override Action GetActionToBeDone()
        {
            this.Action = this.CreateColliderData;
            return this.Action;
        }

        public void CreateColliderData()
        {
            float angle = !this.FollowUnit
                ? Angle.GetBlackBoardValue<float>(this.BelongtoRuntimeTree.GetBlackboard())
                : 0;

            UnitDefine.CreateColliderArgs createColliderArgs =
                ReferencePool.Acquire<UnitDefine.CreateColliderArgs>().Init(this.PrefabABPath, this.NP_TreeConfigId,
                    this.BelongToUnit, angle, this.FollowUnit,
                    this.TargetCollisionRoleCast, this.TargetCollisionRoleCamp, this.TargetCollisionRoleTag,
                    this.Offset, this.TargetPos, OnTriggerEnter, OnTriggerStay, OnTriggerExit);
            UnitFactory.CreateSpecialColliderUnit(BelongToUnit.DomainScene(), createColliderArgs).Forget();
        }
    }
}