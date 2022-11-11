using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace ET
{
    public static class MoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async UniTask<bool> FindPathMoveToAsync(this Unit unit, Vector3 target, float targetRange = 0,
            CancellationToken unitaskCancellationToken = default)
        {
            float speed = unit.GetComponent<NumericComponent>()[NumericType.MoveSpeed] / 100f;
            if (speed < 0.01)
            {
                return true;
            }
            
            ABPath abPath = await unit.GetComponent<NavAgentComponent>().FindPathBetweenA_B(unit.Position, target);

            if (abPath.vectorPath.Count < 1)
            {
                return true;
            }

            bool ret = await unit.GetComponent<MoveComponent>()
                .MoveToAsync(abPath.vectorPath, speed, 100, targetRange, unitaskCancellationToken);

            return ret;
        }

        public static void Stop(this Unit unit)
        {
            unit.GetComponent<MoveComponent>().Stop();
        }
    }
}