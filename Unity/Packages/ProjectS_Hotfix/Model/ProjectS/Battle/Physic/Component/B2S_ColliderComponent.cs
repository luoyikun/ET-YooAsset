//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:17:29
//------------------------------------------------------------

using System.Collections.Generic;
using ET;
using ET.cfg.SkillConfig;
using ET.Client;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 一个碰撞体Component,包含一个碰撞实例所有信息，直接挂载到碰撞体Unit上
    /// 比如诺手Q技能碰撞体UnitA，那么这个B2S_ColliderComponent的Entity就是UnitA，而其中的BelongToUnit就是诺手
    /// </summary>
    public class B2S_ColliderComponent: Entity, IAwake<UnitDefine.CreateColliderArgs>, IDestroy
    {
        /// <summary>
        /// 参数
        /// </summary>
        public UnitDefine.CreateColliderArgs CreateColliderArgs;
        
        /// <summary>
        /// Box2D世界中的刚体
        /// </summary>
        public MonoBridge MonoBridge;

        /// <summary>
        /// 如果碰撞体Unit本身就挂载有SkillCanvasManagerComponent，则往碰撞体Unit身上的Blackboard传递数据，否则直接找监护人去
        /// </summary>
        public SkillCanvasManagerComponent TargetSkillCanvasManager;

        /// <summary>
        /// 归属技能Id
        /// </summary>
        public SkillCanvasConfig SkillCanvasConfig;
    }
}