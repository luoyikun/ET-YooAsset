//此文件格式由工具自动生成
using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("普攻冷却",TitleAlignment = TitleAlignments.Centered)]
    public class NP_TriggerAttackCDAction:NP_ClassForStoreAction
    {        
        public override Action GetActionToBeDone()
        {
            this.Action = this.TriggerAttackCDAction;
            return this.Action;
        }

        public void TriggerAttackCDAction()
        {

        }
    }
}
