using Animancer;
using Cysharp.Threading.Tasks;
using ET.EventType;

namespace ET
{
    public class CommonAttackComponentAwakeSystem : AwakeSystem<CommonAttackComponent_View>
    {
        public override void Awake(CommonAttackComponent_View self)
        {
            Unit unit = self.GetParent<Unit>();
            self.m_AnimationComponent = unit.GetComponent<AnimationComponent>();
            self.m_StackFsmComponent = unit.GetComponent<StackFsmComponent>();
            self.m_MouseTargetSelectorComponent = unit.GetComponent<MouseTargetSelectorComponent>();
            self.m_UserInputComponent = Game.Scene.GetComponent<UserInputComponent>();
        }
    }


    public class CommonAttackComponentUpdateSystem : UpdateSystem<CommonAttackComponent_View>
    {
        public override void Update(CommonAttackComponent_View self)
        {
            // 只有本地玩家才有选择的权力
            if (self.GetParent<Unit>() != self.DomainScene().GetComponent<UnitComponent>().MyUnit)
            {
                return;
            }

            //此处填写Update逻辑
            if (self.m_UserInputComponent.RightMouseDown && self.m_MouseTargetSelectorComponent.TargetUnit != null)
            {
                if (self.m_MouseTargetSelectorComponent.TargetUnit.GetComponent<B2S_RoleCastComponent>()
                        .GetRoleCastToTarget(self.GetParent<Unit>()) ==
                    RoleCast.Adverse)
                {
                    self.m_CachedUnit = self.m_MouseTargetSelectorComponent.TargetUnit;
                    Unit unit = self.GetParent<Unit>();
                    // LSF_CommonAttackCmd lsfCommonAttackCmd =
                    //     ReferencePool.Acquire<LSF_CommonAttackCmd>().Init(unit.Id) as LSF_CommonAttackCmd;
                    // lsfCommonAttackCmd.TargetUnitId = self.m_MouseTargetSelectorComponent.TargetUnit.Id;
                    // self.GetParent<Unit>().BelongToRoom.GetComponent<LSF_Component>()
                    //     .AddCmdToSendQueue(lsfCommonAttackCmd);
                }
            }
        }
    }

    public class CancelAttackFromFsm : AEvent<Unit,EventType.CancelAttackFromFSM>
    {
        protected override async UniTask Run(Unit unit,CancelAttackFromFSM a)
        {
            unit.GetComponent<CommonAttackComponent_View>().CancelCommonAttack();
            await UniTask.CompletedTask;
        }
    }
    
    public class WaitForAttack_View : AEvent<Unit, EventType.WaitForAttack>
    {
        protected override async UniTask Run(Unit unit, WaitForAttack a)
        {
            a.CastUnit.GetComponent<TurnComponent>().Turn(a.TargetUnit.Position);
            a.CastUnit.GetComponent<AnimationComponent>().PlayIdel();
            await UniTask.CompletedTask;
        }
    }

    public static class CommonAttackComponentSystem_View
    {
        public static void CommonAttackStart(this CommonAttackComponent_View self, Unit targetUnit)
        {
            //转向目标Unit
            self.GetParent<Unit>().GetComponent<TurnComponent>().Turn(targetUnit.Position);

            NumericComponent unitAttributesDataComponent =
                self.GetParent<Unit>().GetComponent<NumericComponent>();
            float attackPre = unitAttributesDataComponent[NumericType.Attack] /
                              (1 + unitAttributesDataComponent[NumericType.Attack]);

            //这里假设诺手原始攻击动画0.32s是动画攻击奏效点
            float animationAttackPoint = 0.32f;

            float animationSpeed = animationAttackPoint / attackPre;
            //播放动画，如果动画播放完成还不能进行下一次普攻，则播放空闲动画
            self.m_AnimationComponent.PlayAnimAndReturnIdelFromStart(StateTypes.CommonAttack, speed: animationSpeed,
                fadeMode: FadeMode.FromStart);

            //Game.Scene.GetComponent<SoundComponent>().PlayClip("Darius/Sound_Darius_NormalAttack", 0.4f).Forget();
        }


        public static void CancelCommonAttack(this CommonAttackComponent_View self)
        {
        }
    }
}