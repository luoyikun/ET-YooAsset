using Cysharp.Threading.Tasks;
using ET.cfg.SkillConfig;
using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    public static class UnitFactory
    {
        public static Unit CreateUnit(Scene currentScene, long id, int configId)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(id, configId);
            unitComponent.Add(unit);

            return unit;
        }

        public static async UniTask<Unit> CreatePlayerHero(Scene currentScene, UnitInfo unitInfo)
        {
            Unit unit = CreateUnit(currentScene, unitInfo.UnitId, unitInfo.ConfigId);

            unit.AddComponent<NumericComponent>();
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<StackFsmComponent>();

            unit.AddComponent<NP_SyncComponent>();
            unit.AddComponent<NP_RuntimeTreeManager>();

            //增加Buff管理组件
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<SkillCanvasManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>(unitInfo.RoleCamp, unitInfo.RoleTag);

            unit.AddComponent<CommonAttackComponent_Logic>();
            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<DataModifierComponent>();

            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
            unit.Forward = new Vector3(unitInfo.ForwardX, unitInfo.ForwardY, unitInfo.ForwardZ);

            await Game.EventSystem.PublishAsync(unit, new EventType.AfterUnitCreate_Logic()
            {
                UnitConfigId = unitInfo.ConfigId,
                UnitName = "还是我的Darius"
            });

            return unit;
        }

        public static async UniTask<Unit> CreateMonster(Scene currentScene, UnitInfo unitInfo)
        {
            Unit unit = CreateUnit(currentScene, unitInfo.UnitId, unitInfo.ConfigId);

            unit.AddComponent<NumericComponent>();
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<StackFsmComponent>();

            unit.AddComponent<NP_SyncComponent>();
            unit.AddComponent<NP_RuntimeTreeManager>();

            //增加Buff管理组件
            unit.AddComponent<BuffManagerComponent>();
            unit.AddComponent<SkillCanvasManagerComponent>();
            unit.AddComponent<B2S_RoleCastComponent, RoleCamp, RoleTag>(unitInfo.RoleCamp, unitInfo.RoleTag);

            unit.AddComponent<CastDamageComponent>();
            unit.AddComponent<ReceiveDamageComponent>();
            unit.AddComponent<DataModifierComponent>();

            unit.Position = new Vector3(unitInfo.X, unitInfo.Y, unitInfo.Z);
            unit.Forward = new Vector3(unitInfo.ForwardX, unitInfo.ForwardY, unitInfo.ForwardZ);

            await Game.EventSystem.PublishAsync(unit, new EventType.AfterUnitCreate_Logic()
            {
                IsMonest = true,
                UnitConfigId = unitInfo.ConfigId,
                UnitName = "怪物Darius"
            });

            return unit;
        }

        /// <summary>
        /// 创建碰撞体
        /// </summary>
        /// <param name="room">归属的房间</param>
        /// <param name="belongToUnit">归属的Unit</param>
        /// <param name="colliderDataConfigId">碰撞体数据表Id</param>
        /// <param name="collisionRelationDataConfigId">碰撞关系数据表Id</param>
        /// <param name="colliderNPBehaveTreeIdInExcel">碰撞体的行为树Id</param>
        /// <returns></returns>
        public static async UniTask<Unit> CreateSpecialColliderUnit(Scene currentScene,
            UnitDefine.CreateColliderArgs createColliderArgs)
        {
            //为碰撞体新建一个Unit
            Unit b2sColliderEntity =
                CreateUnit(currentScene, IdGenerater.Instance.GenerateUnitId(currentScene.Zone), 0);

            await Game.EventSystem.PublishAsync<Unit, EventType.CreateColliderGameObject>(b2sColliderEntity,
                new CreateColliderGameObject()
                    { CreateColliderArgs = createColliderArgs, Type = YooAssetProxy.YooAssetResType.Effect });

            if (createColliderArgs.NP_TreeConfigId != 0)
            {
                b2sColliderEntity.AddComponent<NP_SyncComponent>();
                b2sColliderEntity.AddComponent<NP_RuntimeTreeManager>();
                b2sColliderEntity.AddComponent<SkillCanvasManagerComponent>();

                SkillCanvasConfig skillCanvasConfig =
                    ConfigComponent.Instance.AllConfigTables.TbSkillCanvas.Get(createColliderArgs.NP_TreeConfigId);

                NP_RuntimeTreeFactory.CreateSkillNpRuntimeTree(b2sColliderEntity, skillCanvasConfig.NPBehaveId,
                        skillCanvasConfig.BelongToSkillId)
                    .Start();
            }

            b2sColliderEntity.AddComponent<B2S_ColliderComponent, UnitDefine.CreateColliderArgs>(createColliderArgs);

            return b2sColliderEntity;
        }
    }
}