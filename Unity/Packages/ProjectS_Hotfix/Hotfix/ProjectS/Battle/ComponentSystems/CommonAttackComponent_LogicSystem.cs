//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月21日 15:15:48
//------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ET.EventType;
using NPBehave;
using UnityEngine;

namespace ET
{
    public class CommonAttackComponent_LogicAwakeSystem : AwakeSystem<CommonAttackComponent_Logic>
    {
        public override void Awake(CommonAttackComponent_Logic self)
        {
            Unit unit = self.GetParent<Unit>();

            //此处填写Awake逻辑
            self.StackFsmComponent = unit.GetComponent<StackFsmComponent>();
            self.CancellationTokenSource = new CancellationTokenSource();

            CDInfo attackCDInfo = ReferencePool.Acquire<CDInfo>();
            attackCDInfo.Name = "CommonAttack";
            attackCDInfo.Interval = 750;

            CDInfo moveCDInfo = ReferencePool.Acquire<CDInfo>();
            moveCDInfo.Name = "MoveToAttack";
            moveCDInfo.Interval = 300;

            CDComponent.Instance.AddCDData(unit.Id, attackCDInfo);
            CDComponent.Instance.AddCDData(unit.Id, moveCDInfo);
        }
    }

    public class CommonAttackComponentDestroySystem : DestroySystem<CommonAttackComponent_Logic>
    {
        public override void Destroy(CommonAttackComponent_Logic self)
        {
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource?.Dispose();

            self.ReSetAttackReplaceInfo();
            self.ReSetCancelAttackReplaceInfo();
        }
    }


    public class CancelAttackFromFsm_Event : AEvent<Unit, EventType.CancelAttackFromFSM>
    {
        protected override async UniTask Run(Unit unit, CancelAttackFromFSM a)
        {
            if (a.ResetAttackTarget)
            {
                unit.GetComponent<CommonAttackComponent_Logic>().CancelCommonAttack();
            }
            else
            {
                unit.GetComponent<CommonAttackComponent_Logic>().CancelCommonAttackWithOutResetTarget();
            }

            await UniTask.CompletedTask;
        }
    }

    public static class CommonAttackComponentUtilities
    {
        public static void SetAttackTarget(this CommonAttackComponent_Logic self, Unit targetUnit)
        {
            if (targetUnit == null)
            {
                Log.Error("普攻组件接收到的targetUnit为null");
                return;
            }

            if (targetUnit.GetComponent<B2S_RoleCastComponent>().GetRoleCastToTarget(self.GetParent<Unit>()) ==
                RoleCast.Adverse)
            {
                if (self.CachedUnitForAttack != targetUnit)
                {
                    self.CancelCommonAttack();
                }

                self.CachedUnitForAttack = targetUnit;

                self.StackFsmComponent.ChangeState<CommonAttackState>(StateTypes.CommonAttack, "CommonAttack", 1);
            }
        }

        private static async UniTask StartCommonAttack(this CommonAttackComponent_Logic self)
        {
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource?.Dispose();
            //如果有要执行攻击流程替换的内容，就执行替换流程
            if (self.HasAttackReplaceInfo())
            {
                Unit unit = self.GetParent<Unit>();

                NP_RuntimeTree npRuntimeTree = unit.GetComponent<NP_RuntimeTreeManager>()
                    .GetTreeByRuntimeID(self.AttackReplaceNPTreeId);
                Blackboard blackboard = npRuntimeTree.GetBlackboard();

                blackboard.Set(self.AttackReplaceBB.BBKey, true);
                blackboard.Set(self.CancelAttackReplaceBB.BBKey, false);

                blackboard.Set("NormalAttackUnitIds", new List<long>() { self.CachedUnitForAttack.Id });

                CDInfo commonAttackCDInfo = CDComponent.Instance.GetCDData(unit.Id, "CommonAttack");
                await self.GetParent<Unit>().DomainScene().GetComponent<LSF_TimerComponent>()
                    .WaitAsync(commonAttackCDInfo.Interval, self.CancellationTokenSource.Token);
            }
            else
            {
                await self.CommonAttack_Internal();
            }

            //此次攻击完成
            self.CancellationTokenSource = null;
        }

        private static async UniTask CommonAttack_Internal(this CommonAttackComponent_Logic self)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponent heroDataComponent = unit.GetComponent<NumericComponent>();
            float attackPre = heroDataComponent[NumericType.AttackAdd] / heroDataComponent[NumericType.AttackAdd];
            float attackSpeed = heroDataComponent[NumericType.AttackSpeed];

            UnitComponent unitComponent = unit.DomainScene().GetComponent<UnitComponent>();
            Game.EventSystem.Publish(unit, new EventType.CommonAttack()
            {
                AttackCast = unit,
                AttackTarget = self.CachedUnitForAttack
            });

            //播放动画，如果动画播放完成还不能进行下一次普攻，则播放空闲动画
            if (!await self.GetParent<Unit>().DomainScene().GetComponent<LSF_TimerComponent>()
                    .WaitAsync((long)(attackPre * 1000), self.CancellationTokenSource.Token))
            {
                return;
            }

            // TODO 客户端不进行伤害计算，由服务端发回
            DamageData damageData = ReferencePool.Acquire<DamageData>().InitData(
                BuffDamageTypes.PhysicalSingle | BuffDamageTypes.CommonAttack,
                heroDataComponent[NumericType.Attack], unit, self.CachedUnitForAttack);

            unit.GetComponent<CastDamageComponent>().BaptismDamageData(damageData);
            float finalDamage = self.CachedUnitForAttack.GetComponent<ReceiveDamageComponent>()
                .BaptismDamageData(damageData);

            if (finalDamage >= 0)
            {
                self.CachedUnitForAttack.GetComponent<NumericComponent>()
                    .ApplyChange(NumericType.Hp, -finalDamage);

                BattleEventSystemComponent battleEventSystemComponent =
                    unit.DomainScene().GetComponent<BattleEventSystemComponent>();

                //抛出伤害事件，需要监听伤害的buff（比如吸血buff）需要监听此事件
                battleEventSystemComponent.Run($"ExcuteDamage{unit.Id}", damageData);
                //抛出受伤事件，需要监听受伤的Buff（例如反甲）需要监听此事件
                battleEventSystemComponent.Run($"TakeDamage{self.CachedUnitForAttack.Id}", damageData);
            }

            CDComponent.Instance.TriggerCD(unit.Id, "CommonAttack");
            CDInfo commonAttackCDInfo = CDComponent.Instance.GetCDData(unit.Id, "CommonAttack");
            commonAttackCDInfo.Interval = (long)(1 / attackSpeed - attackPre) * 1000;

            List<NP_RuntimeTree> targetSkillCanvas =
                unit.GetComponent<SkillCanvasManagerComponent>().GetSkillCanvas(10001);
            foreach (var skillCanva in targetSkillCanvas)
            {
                skillCanva.GetBlackboard().Set("CastNormalAttack", true);
                skillCanva.GetBlackboard().Set("NormalAttackUnitIds", new List<long>() { self.CachedUnitForAttack.Id });
            }

            await self.GetParent<Unit>().DomainScene().GetComponent<LSF_TimerComponent>()
                .WaitAsync(commonAttackCDInfo.Interval, self.CancellationTokenSource.Token);
        }

        public static void FixedUpdate(this CommonAttackComponent_Logic self)
        {
            Unit unit = self.GetParent<Unit>();

            if (unit.GetComponent<StackFsmComponent>().GetCurrentFsmState().StateTypes == StateTypes.CommonAttack)
            {
                if (self.CachedUnitForAttack != null && !self.CachedUnitForAttack.IsDisposed)
                {
                    Vector3 selfUnitPos = unit.Position;
                    double distance = Vector3.Distance(selfUnitPos, self.CachedUnitForAttack.Position);
                    float attackRange = unit.GetComponent<NumericComponent>()
                        [NumericType.AttackRange] / 100;

                    //目标距离大于当前攻击距离会先进行寻路，这里的1.75为175码
                    if (distance - attackRange >= 0.1f)
                    {
                        if (!CDComponent.Instance.GetCDResult(unit.Id, "MoveToAttack")) return;
                        CDComponent.Instance.TriggerCD(unit.Id, "MoveToAttack");

                        CommonAttackState commonAttackState = ReferencePool.Acquire<CommonAttackState>();
                        commonAttackState.SetData(StateTypes.CommonAttack, "CommonAttack", 1);
                        // unit.NavigateTodoSomething(self.CachedUnitForAttack.Position, 1.75f, commonAttackState)
                        //     .Forget();
                    }
                    else
                    {
                        //目标不为空，且处于攻击状态，且上次攻击已完成或取消
                        if ((self.CancellationTokenSource == null ||
                             self.CancellationTokenSource.IsCancellationRequested))
                        {
                            if (CDComponent.Instance.GetCDResult(unit.Id, "CommonAttack"))
                                self.StartCommonAttack().Forget();
                            else // 说明还不能进行下一次普攻，就罚站
                            {
#if !SERVER
                                //TODO 可能服务端也会有同步转向的需求
                                Game.EventSystem.Publish(unit, new WaitForAttack()
                                    { CastUnit = unit, TargetUnit = self.CachedUnitForAttack });
#endif
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取消攻击但不重置攻击对象
        /// </summary>
        public static void CancelCommonAttackWithOutResetTarget(this CommonAttackComponent_Logic self)
        {
            CancellationTokenSource token = self.CancellationTokenSource;
            self.CancellationTokenSource = null;
            token?.Cancel();

            if (self.HasCancelAttackReplaceInfo())
            {
                Unit unit = self.GetParent<Unit>();
                unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(self.CancelAttackReplaceNPTreeId)
                    .GetBlackboard()
                    .Set(self.AttackReplaceBB.BBKey, false);
                unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(self.CancelAttackReplaceNPTreeId)
                    .GetBlackboard()
                    .Set(self.CancelAttackReplaceBB.BBKey, true);
            }
        }

        /// <summary>
        /// 取消攻击但不重置攻击对象，会重置普攻CD，可以立即进行下一次普攻
        /// </summary>
        public static void CancelCommonAttackWithOutResetTarget_ResetAttackCD(this CommonAttackComponent_Logic self)
        {
            self.CancellationTokenSource?.Cancel();
            self.CancellationTokenSource = null;

            if (self.HasCancelAttackReplaceInfo())
            {
                Unit unit = self.GetParent<Unit>();
                unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(self.CancelAttackReplaceNPTreeId)
                    .GetBlackboard()
                    .Set(self.AttackReplaceBB.BBKey, false);
                unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(self.CancelAttackReplaceNPTreeId)
                    .GetBlackboard()
                    .Set(self.CancelAttackReplaceBB.BBKey, true);
            }

            CDComponent.Instance.ResetCD(self.GetParent<Unit>().Id, "CommonAttack");
        }

        /// <summary>
        /// 取消攻击并且重置攻击对象
        /// </summary>
        public static void CancelCommonAttack(this CommonAttackComponent_Logic self)
        {
            self.CancelCommonAttackWithOutResetTarget();
            self.CachedUnitForAttack = null;
        }
    }
}