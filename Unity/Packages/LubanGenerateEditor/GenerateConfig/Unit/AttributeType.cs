//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ET.Editor.Unit
{

    public enum AttributeType
    {
        /// <summary>
        /// 生命
        /// </summary>
        HP = 1,
        /// <summary>
        /// 魔法
        /// </summary>
        MP = 2,
        /// <summary>
        /// 攻击力
        /// </summary>
        Attack = 3,
        /// <summary>
        /// 移速
        /// </summary>
        MoveSpeed = 4,
        /// <summary>
        /// 攻速
        /// </summary>
        AttackSpeed = 5,
    }

    public static partial class AttributeType_Metadata
    {
        public static readonly Bright.Config.EditorEnumItemInfo HP = new Bright.Config.EditorEnumItemInfo("HP", "生命", 1, "生命");
        public static readonly Bright.Config.EditorEnumItemInfo MP = new Bright.Config.EditorEnumItemInfo("MP", "魔法", 2, "魔法");
        public static readonly Bright.Config.EditorEnumItemInfo Attack = new Bright.Config.EditorEnumItemInfo("Attack", "攻击力", 3, "攻击力");
        public static readonly Bright.Config.EditorEnumItemInfo MoveSpeed = new Bright.Config.EditorEnumItemInfo("MoveSpeed", "移速", 4, "移速");
        public static readonly Bright.Config.EditorEnumItemInfo AttackSpeed = new Bright.Config.EditorEnumItemInfo("AttackSpeed", "攻速", 5, "攻速");

        private static readonly System.Collections.Generic.List<Bright.Config.EditorEnumItemInfo> __items = new System.Collections.Generic.List<Bright.Config.EditorEnumItemInfo>
        {
            HP,
            MP,
            Attack,
            MoveSpeed,
            AttackSpeed,
        };

        public static System.Collections.Generic.List<Bright.Config.EditorEnumItemInfo> GetItems() => __items;

        public static Bright.Config.EditorEnumItemInfo GetByName(string name)
        {
            return __items.Find(c => c.Name == name);
        }

        public static Bright.Config.EditorEnumItemInfo GetByNameOrAlias(string name)
        {
            return __items.Find(c => c.Name == name || c.Alias == name);
        }

        public static Bright.Config.EditorEnumItemInfo GetByValue(int value)
        {
            return __items.Find(c => c.Value == value);
        }
    }
}
