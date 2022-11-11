//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 15:07:27
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ET
{
    /// <summary>
    /// 特效组件，用于管理Unit身上的特效
    /// </summary>
    public class EffectComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<string, GameObject> AllEffects = new Dictionary<string, GameObject>();

        /// <summary>
        /// 特效组，用于处理互斥特效，例如诺克的一个血滴，两个血滴，三个血滴这种，里面的数据应由excel表来配置
        /// </summary>
        public List<string> effectGroup = new List<string>
        {
            "Darius_Passive_Bleed_Effect_1",
            "Darius_Passive_Bleed_Effect_2",
            "Darius_Passive_Bleed_Effect_3",
            "Darius_Passive_Bleed_Effect_4",
            "Darius_Passive_Bleed_Effect_5"
        };
    }
}