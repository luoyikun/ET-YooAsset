// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月16日, 星期六
// --------------------------

using Cysharp.Threading.Tasks;
using Pathfinding;

namespace ET
{
    /// <summary>
    /// 添加此组件代表拥有寻路能力
    /// </summary>
    public class NavAgentComponent : Entity, IAwake
    {
        /// <summary>
        /// 多线程适配
        /// </summary>
        public UniTaskCompletionSource<ABPath> PathFindResultTcs = new UniTaskCompletionSource<ABPath>();

        public void OnPathFindFinish(Path path)
        {
            PathFindResultTcs.TrySetResult(path as ABPath);
        }
    }
}