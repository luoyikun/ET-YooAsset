//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 21:13:00
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("打印信息",TitleAlignment = TitleAlignments.Centered)]
    public class NP_LogAction:NP_ClassForStoreAction
    {
        [LabelText("信息")]
        public string LogInfo;
        
        public override Action GetActionToBeDone()
        {
            this.Action = this.TestLog;
            return this.Action;
        }

        public void TestLog()
        {
            Log.Info(LogInfo);
        }
    }
}