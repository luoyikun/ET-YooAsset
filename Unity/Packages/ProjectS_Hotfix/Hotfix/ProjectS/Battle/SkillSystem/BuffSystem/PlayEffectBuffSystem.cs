//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 14:15:24
//------------------------------------------------------------

using UnityEngine;

namespace ET
{
    public class PlayEffectBuffSystem : ABuffSystemBase<PlayEffectBuffData>
    {
        public override void OnExecute(uint currentFrame)
        {
            Game.EventSystem.Publish(this.GetBuffTarget(), new EventType.PlayEffectBuffSystemExcuteEvent()
            {
                PlayEffectBuffData = GetBuffDataWithTType,
                CurrentOverlay = this.CurrentOverlay
            });

            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystemComponent>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }

        public override void OnFinished(uint currentFrame)
        {
            Game.EventSystem.Publish(this.GetBuffTarget(), new EventType.PlayEffectBuffSystemFinishEvent()
            {
                PlayEffectBuffData = GetBuffDataWithTType,
                CurrentOverlay = this.CurrentOverlay
            });
        }

        public override void OnRefreshed(uint currentFrame)
        {
            Game.EventSystem.Publish(this.GetBuffTarget(), new EventType.PlayEffectBuffSystemExcuteEvent()
            {
                PlayEffectBuffData = GetBuffDataWithTType,
                CurrentOverlay = this.CurrentOverlay
            });

            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystemComponent>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }
    }
}