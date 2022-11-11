namespace ET
{
    public interface IFSMState
    {
        /// <summary>
        /// 尝试进入此状态时需要进行的逻辑，成功进入此状态则返回true，否则返回false
        /// </summary>
        /// <returns></returns>
        bool TryEnter(StackFsmComponent stackFsmComponent);
        
        /// <summary>
        /// 正式进入此状态时需要进行的逻辑
        /// </summary>
        void OnEnter(StackFsmComponent stackFsmComponent);
        
        /// <summary>
        /// 离开此状态时需要进行的逻辑
        /// </summary>
        void OnExit(StackFsmComponent stackFsmComponent);

        /// <summary>
        /// 状态被移除时调用
        /// </summary>
        /// <param name="stackFsmComponent"></param>
        void OnRemoved(StackFsmComponent stackFsmComponent);
    }
}