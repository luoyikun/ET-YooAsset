//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月13日 17:30:29
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;

namespace ET
{
    public abstract class AFsmStateBase: IFSMState, IReference
    {
        /// <summary>
        /// 状态类型
        /// </summary>
        [LabelText("状态类型")]
        public StateTypes StateTypes;

        /// <summary>
        /// 状态名称
        /// </summary>
        [LabelText("状态名称")]
        public string StateName;

        /// <summary>
        /// 状态的优先级，值越大，优先级越高。
        /// </summary>
        [LabelText("状态的优先级")]
        public int Priority;

        public AFsmStateBase()
        {
        }

        public void SetData(StateTypes stateTypes, string stateName, int priority)
        {
            StateTypes = stateTypes;
            StateName = stateName;
            this.Priority = priority;
        }

        public virtual bool TryEnter(StackFsmComponent stackFsmComponent)
        {
            if (stackFsmComponent.CheckConflictState(GetConflictStateTypeses()))
            {
                return false;
            }

            return true;
        }

        public abstract StateTypes GetConflictStateTypeses();

        public abstract void OnEnter(StackFsmComponent stackFsmComponent);

        public abstract void OnExit(StackFsmComponent stackFsmComponent);

        /// <summary>
        /// 状态移除时调用
        /// </summary>
        /// <param name="stackFsmComponent"></param>
        public abstract void OnRemoved(StackFsmComponent stackFsmComponent);

        public virtual void Clear()
        {
        }
    }
}