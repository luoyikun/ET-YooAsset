// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月16日, 星期六
// --------------------------

using Cysharp.Threading.Tasks;
using ET.Client;
using Pathfinding;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// game scene需要挂载的组件，是整个寻路引擎的单例
    /// </summary>
    public class PathFindComponent : Entity, IAwake, IDestroy
    {
        public AstarPath PathFindInstance;
    }
}