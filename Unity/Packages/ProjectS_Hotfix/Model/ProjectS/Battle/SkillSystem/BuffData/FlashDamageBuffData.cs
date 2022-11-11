//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月18日 16:38:28
//------------------------------------------------------------

using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;

namespace ET
{
    public class FlashDamageBuffData: BuffDataBase
    {
        [BoxGroup("自定义项")]
        [LabelText("伤害类型")]
        public BuffDamageTypes BuffDamageTypes;

        [BoxGroup("自定义项")]
        [LabelText("伤害附带的信息")]
        public string CustomData;

        [BoxGroup("自定义项")]
        [LabelText("预伤害修正")]
        public float DamageFix = 1.0f;
    }
}