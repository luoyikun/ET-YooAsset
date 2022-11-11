//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月1日 15:23:58
//------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class SustainDamageBuffData: BuffDataBase
    {
        [BoxGroup("自定义项")]
        [LabelText("伤害类型")]
        public BuffDamageTypes BuffDamageTypes;
        
        [BoxGroup("自定义项")]
        [LabelText("预伤害修正")]
        public float DamageFix = 1.0f;

        [UnityEngine.Tooltip("1000为1s")]
        [BoxGroup("自定义项")]
        [LabelText("作用间隔")]
        public long WorkInternal = 0;
    }
}