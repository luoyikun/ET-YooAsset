using System;
using System.Collections.Generic;

namespace ET
{
    namespace EventType
    {
        public struct ChangeMaterialBuffSystemExcuteEvent
        {
            public ChangeMaterialBuffData ChangeMaterialBuffData;
            public Unit Target;
        }

        public struct ChangeMaterialBuffSystemFinishEvent
        {
            public ChangeMaterialBuffData ChangeMaterialBuffData;
            public Unit Target;
        }

        public struct ChangeRenderAssetBuffSystemExcuteEvent
        {
            public ChangeRenderAssetBuffData ChangeMaterialBuffData;
            public Unit Target;
        }

        public struct ChangeRenderAssetBuffSystemFinishEvent
        {
            public ChangeRenderAssetBuffData ChangeMaterialBuffData;
            public Unit Target;
        }

        public struct PlayEffectBuffSystemExcuteEvent
        {
            public PlayEffectBuffData PlayEffectBuffData;
            public int CurrentOverlay;
        }

        public struct PlayEffectBuffSystemFinishEvent
        {
            public PlayEffectBuffData PlayEffectBuffData;
            public int CurrentOverlay;
        }

        public struct ReplaceAnimBuffSystemExcuteEvent
        {
            public ReplaceAnimBuffData ReplaceAnimBuffData;
            public Unit Target;
            public Dictionary<string, string> ReplacedAnimData;
        }

        public struct RepalceAnimBuffSystemFinishEvent
        {
            public ReplaceAnimBuffData ReplaceAnimBuffData;
            public Unit Target;
            public Dictionary<string, string> ReplacedAnimData;
        }

        public struct NumericApplyChangeValue
        {
            public NumericType NumericType;
            public float ChangedValue;
        }

        public struct PlayRunAnimationByMoveSpeed
        {
            public float Speed;
            public Unit Unit;
        }

        public struct CancelMoveFromFSM
        {
        }

        public struct CancelAttackFromFSM
        {
            public bool ResetAttackTarget;
        }

        public struct FSMStateChanged_PlayAnim
        {
            
        }
        
        /// <summary>
        /// 等待下次普攻可以发出
        /// </summary>
        public struct WaitForAttack
        {
            public Unit CastUnit;
            public Unit TargetUnit;
        }
        
        /// <summary>
        /// 修改对象的属性，用于处理具体的改变数值
        /// 例如服务端发送了一条扣血（50）消息
        /// Numeric处理当前血量（例如当前血量为100 - 50 = 50）事件
        /// 而这个事件则处理改变了50这个事件，比如出现50飘血字样
        /// </summary>
        public struct ChangeUnitAttribute
        {
            public Unit Unit;
            public NumericType NumericType;
            public float ChangeValue;
        }

        public struct CommonAttack
        {
            public Unit AttackCast;
            public Unit AttackTarget;
        }

        public struct CancelCommonAttack
        {
            public Unit AttackCast;
        }
    }
}