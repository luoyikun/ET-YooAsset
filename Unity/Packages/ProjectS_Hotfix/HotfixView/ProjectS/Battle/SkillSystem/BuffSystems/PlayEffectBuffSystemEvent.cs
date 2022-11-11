using Cysharp.Threading.Tasks;
using ET.EventType;
using UnityEngine;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class PlayEffectBuffSystemEvent : AEvent<Unit, EventType.PlayEffectBuffSystemExcuteEvent>
    {
        protected override async UniTask Run(Unit unit, PlayEffectBuffSystemExcuteEvent a)
        {
            PlayEffectBuffData playEffectBuffData = a.PlayEffectBuffData;
            string targetEffectName = playEffectBuffData.EffectName;

            if (playEffectBuffData.CanChangeNameByCurrentOverlay)
            {
                targetEffectName = $"{playEffectBuffData.EffectName}{a.CurrentOverlay}";
                //Log.Info($"播放{targetEffectName}");
            }

            //如果想要播放的特效正在播放，就返回
            if (unit.GetComponent<EffectComponent>().CheckState(targetEffectName)) return;
            
            GameObject effectUnit =
                await GameObjectPoolComponent.Instance.FetchGameObject(targetEffectName,
                    YooAssetProxy.YooAssetResType.Effect);

            if (playEffectBuffData.FollowUnit)
            {
                effectUnit.transform.SetParent(unit.GetComponent<UnitTransformComponent>()
                    .GetTranform(playEffectBuffData.PosType));

                effectUnit.transform.localPosition = Vector3.zero;
            }

            unit.GetComponent<EffectComponent>().Play(targetEffectName, effectUnit);

            await UniTask.CompletedTask;
        }
    }

    [Event(SceneType.SingleGame)]
    public class PlayEffectBuffSystemEvent1 : AEvent<Unit, EventType.PlayEffectBuffSystemFinishEvent>
    {
        protected override async UniTask Run(Unit unit, PlayEffectBuffSystemFinishEvent a)
        {
            PlayEffectBuffData playEffectBuffData = a.PlayEffectBuffData;
            string targetEffectName = playEffectBuffData.EffectName;
            if (playEffectBuffData.CanChangeNameByCurrentOverlay)
            {
                targetEffectName = $"{playEffectBuffData.EffectName}{a.CurrentOverlay}";
            }

            unit.GetComponent<EffectComponent>()
                .Remove(targetEffectName);

            await UniTask.CompletedTask;
        }
    }
}