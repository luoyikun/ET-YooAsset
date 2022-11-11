using Cysharp.Threading.Tasks;
using ET.cfg.UnitConfig;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.SingleGame)]
    public class AfterUnitCreate_CreateUnitView : AEvent<Unit, EventType.AfterUnitCreate_Logic>
    {
        protected override async UniTask Run(Unit unit, EventType.AfterUnitCreate_Logic args)
        {
            UnitResConfig unitResConfig = ConfigComponent.Instance.AllConfigTables.TbUnitRes[args.UnitConfigId];

            GameObject go = await unit
                .AddComponent<GameObjectComponent, YooAssetProxy.YooAssetResType, string>(
                    YooAssetProxy.YooAssetResType.Unit, unitResConfig.PrefabName)
                .CreateGameObjectInternal();

            go.transform.position = unit.Position;
            go.transform.rotation = unit.Rotation;
            go.name = args.UnitName;

            unit.AddComponent<AnimationComponent>();
            unit.AddComponent<UnitTransformComponent>();
            unit.AddComponent<EffectComponent>();
            unit.AddComponent<NavAgentComponent>();
            unit.GetComponent<SkillCanvasManagerComponent>().InitUnitPresetSkillCanavs();

            if (!args.IsMonest)
            {
                unit.AddComponent<MouseTargetSelectorComponent>();
                unit.AddComponent<MapClickCompoent>();
                unit.AddComponent<PlayerHeroControllerComponent>();
            }

            await UniTask.CompletedTask;
        }
    }
}