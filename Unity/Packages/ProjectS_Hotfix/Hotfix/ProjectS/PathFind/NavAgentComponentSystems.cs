// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月16日, 星期六
// --------------------------

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace ET
{
    public class NavAgentComponentAwakeSystem : AwakeSystem<NavAgentComponent>
    {
        public override void Awake(NavAgentComponent self)
        {
        }
    }

    public static class NavAgentComponentSystems
    {
        public static async UniTask<ABPath> FindPathBetweenA_B(this NavAgentComponent self, Vector3 start, Vector3 end)
        {
            self.PathFindResultTcs = new UniTaskCompletionSource<ABPath>();
            
            ABPath abPath = ABPath.Construct(start, end, self.OnPathFindFinish);
            AstarPath.StartPath(abPath);

            abPath = await self.PathFindResultTcs.Task;
            
            PathModifyHelper.StartEndModify(abPath);
            PathModifyHelper.FunnelModify(abPath);
            
            return abPath;
        }
    }
}