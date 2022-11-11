// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月17日, 星期日
// --------------------------

using ET.cfg.UnitConfig;

namespace ET
{
    public class NumericComponentSystems_AwakeSystem0 : AwakeSystem<NumericComponent>
    {
        public override void Awake(NumericComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            UnitBaseConfig unitBaseConfig = ConfigComponent.Instance.AllConfigTables.TbUnitBase.Get(unit.ConfigId);

            self.SetValueWithoutBroadCast(NumericType.Hp, unitBaseConfig.AttributeConfigId_Ref.HP);
            self.SetValueWithoutBroadCast(NumericType.Mp, unitBaseConfig.AttributeConfigId_Ref.MP);
            self.SetValueWithoutBroadCast(NumericType.Attack, unitBaseConfig.AttributeConfigId_Ref.Attack);
            self.SetValueWithoutBroadCast(NumericType.AttackSpeed, unitBaseConfig.AttributeConfigId_Ref.AttackSpeed);
            self.SetValueWithoutBroadCast(NumericType.MoveSpeed, unitBaseConfig.AttributeConfigId_Ref.MoveSpeed);
        }
    }

    public class NumericComponentSystems_DestroySystem0 : DestroySystem<NumericComponent>
    {
        public override void Destroy(NumericComponent self)
        {
            self.OriNumericDic.Clear();
            self.NumericDic.Clear();
        }
    }


    public static class NumericComponentSystems_Utilities
    {
        /// <summary>
        /// 适配变化
        /// </summary>
        public static void ApplyChange(this NumericComponent self, NumericType numericType, float changedValue)
        {
            Unit unit = self.GetParent<Unit>();
            Game.EventSystem.Publish(unit, new EventType.NumericApplyChangeValue()
                { ChangedValue = changedValue, NumericType = numericType });
            self[numericType] += changedValue;
        }
    }
}