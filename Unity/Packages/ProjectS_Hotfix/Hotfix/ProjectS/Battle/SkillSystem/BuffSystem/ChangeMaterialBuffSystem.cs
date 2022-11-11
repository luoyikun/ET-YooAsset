//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 19:22:48
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ChangeMaterialBuffSystem: ABuffSystemBase<ChangeMaterialBuffData>
    {
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private long m_SelfNextimer;
        public override void OnExecute(uint currentFrame)
        {
            Game.EventSystem.Publish(this.GetBuffTarget(), new EventType.ChangeMaterialBuffSystemExcuteEvent()
                {ChangeMaterialBuffData = GetBuffDataWithTType, Target = this.GetBuffTarget()});
        }

        public override void OnFinished(uint currentFrame)
        {
            Game.EventSystem.Publish(this.GetBuffTarget(), new EventType.ChangeMaterialBuffSystemFinishEvent()
                {ChangeMaterialBuffData = GetBuffDataWithTType, Target = this.GetBuffTarget()});
        }

    }
}