//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月24日 21:26:46
//------------------------------------------------------------

using System.Collections.Generic;

namespace ET
{
    public class ReplaceAnimBuffSystem: ABuffSystemBase<ReplaceAnimBuffData>
    {
#if !SERVER 
        /// <summary>
        /// 被替换下来的动画信息
        /// </summary>
        private Dictionary<string, string> m_ReplacedAnimData = new Dictionary<string, string>();

        public override void OnExecute(uint currentFrame)
        {
            Game.EventSystem.Publish(this.GetBuffTarget(),new EventType.ReplaceAnimBuffSystemExcuteEvent()
            {
                ReplaceAnimBuffData = this.GetBuffDataWithTType, Target = this.GetBuffTarget(),
                ReplacedAnimData = m_ReplacedAnimData
            });
        }

        public override void OnFinished(uint currentFrame)
        {
            Game.EventSystem.Publish(this.GetBuffTarget(),new EventType.RepalceAnimBuffSystemFinishEvent()
            {
                ReplaceAnimBuffData = this.GetBuffDataWithTType, Target = this.GetBuffTarget(),
                ReplacedAnimData = m_ReplacedAnimData
            });
        }
#else
        public override void OnExecute(uint currentFrame)
        {

        }
#endif
    }
}