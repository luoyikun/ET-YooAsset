﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class MoveComponentDestroySystem : DestroySystem<MoveComponent>
    {
        public override void Destroy(MoveComponent self)
        {
            self.Clear();
        }
    }
    
    public class MoveComponentFxiedUpdateSystem : FixedUpdateSystem<MoveComponent>
    {
        public override void FixedUpdate(MoveComponent self)
        {
            if (self.ShouldMove)
            {
                self.MoveForward(GlobalDefine.FixedUpdateTargetDTTime_Long, false);
            }
        }
    }

    [ObjectSystem]
    public class MoveComponentAwakeSystem : AwakeSystem<MoveComponent>
    {
        public override void Awake(MoveComponent self)
        {
            self.StartTime = 0;
            self.StartPos = Vector3.zero;
            self.NeedTime = 0;
            self.Callback = null;
            self.Targets.Clear();
            self.Speed = 0;
            self.NextPointIndex = 0;
            self.TurnTime = 0;
        }
    }

    public static class MoveComponentSystem
    {
        public static bool IsArrived(this MoveComponent self)
        {
            return self.Targets.Count == 0;
        }

        public static bool ChangeSpeed(this MoveComponent self, float speed)
        {
            if (self.IsArrived())
            {
                return false;
            }

            if (speed < 0.0001)
            {
                return false;
            }

            Unit unit = self.GetParent<Unit>();
            using (ListComponent<Vector3> path = ListComponent<Vector3>.Create())
            {
                path.Add(unit.Position); // 第一个是Unit的pos
                for (int i = self.NextPointIndex; i < self.Targets.Count; ++i)
                {
                    path.Add(self.Targets[i]);
                }

                self.Stop();
                self.MoveToAsync(path, speed).Forget();
            }

            return true;
        }

        public static async UniTask<bool> MoveToAsync(this MoveComponent self, List<Vector3> target, float speed,
            int turnTime = 100, float targetRange = 0, CancellationToken cancellationToken = default)
        {
            if (!self.GetParent<Unit>().GetComponent<StackFsmComponent>()
                    .ChangeState<NavigateState>(StateTypes.Run, "Navigate", 1)) return false;

            self.Stop();
            self.TargetRange = targetRange;

            self.Targets.Add(self.GetParent<Unit>().Position);
            foreach (Vector3 v in target)
            {
                self.Targets.Add(v);
            }

            self.IsTurnHorizontal = true;
            self.TurnTime = turnTime;
            self.Speed = speed;

            UniTaskCompletionSource<bool> tcs = new UniTaskCompletionSource<bool>();

            self.Callback = (ret) => { tcs.TrySetResult(ret); };

            Game.EventSystem.Publish(self.GetParent<Unit>(), new EventType.MoveStart() { Speed = speed });

            self.StartMove();

            void CancelAction()
            {
                self.Stop();
            }

            bool moveRet;

            cancellationToken.Register(CancelAction);
            moveRet = await tcs.Task;

            if (moveRet)
            {
                Game.EventSystem.Publish(self.GetParent<Unit>(), new EventType.MoveStop());
                Game.EventSystem.Publish(self.GetParent<Unit>(), new EventType.FSMStateChanged_PlayAnim());
            }

            return moveRet;
        }

        public static void MoveForward(this MoveComponent self, long deltaTime, bool needCancel)
        {
            Unit unit = self.GetParent<Unit>();

            long moveTime = self.AccumulateTime += deltaTime;

            while (true)
            {
                if (moveTime <= 0)
                {
                    return;
                }

                // 计算位置插值
                if (moveTime >= self.NeedTime)
                {
                    unit.Position = self.NextTarget;
                    if (self.TurnTime > 0)
                    {
                        unit.Rotation = self.To;
                    }
                }
                else
                {
                    // 计算位置插值
                    float amount = moveTime * 1f / self.NeedTime;
                    if (amount > 0)
                    {
                        Vector3 newPos = Vector3.Lerp(self.StartPos, self.NextTarget, amount);
                        unit.Position = newPos;
                    }

                    // 计算方向插值
                    if (self.TurnTime > 0)
                    {
                        amount = moveTime * 1f / self.TurnTime;
                        Quaternion q = Quaternion.Slerp(self.From, self.To, amount);
                        unit.Rotation = q;
                    }
                }

                moveTime -= self.NeedTime;

                // 如果抵达了目标范围，强行让客户端停止
                if (Vector3.Distance(unit.Position, self.FinalTarget) - self.TargetRange <= 0.0001f)
                {
                    unit.Rotation = self.To;

                    Action<bool> callback = self.Callback;
                    self.Callback = null;

                    self.Clear();
                    callback?.Invoke(true);
                    return;
                }

                // 表示这个点还没走完，等下一帧再来
                if (moveTime < 0)
                {
                    return;
                }

                // 如果是最后一个点
                if (self.NextPointIndex >= self.Targets.Count - 1)
                {
                    unit.Position = self.NextTarget;
                    unit.Rotation = self.To;

                    Action<bool> callback = self.Callback;
                    self.Callback = null;

                    self.Clear();
                    callback?.Invoke(!needCancel);
                    return;
                }

                self.SetNextTarget();
            }
        }

        private static void StartMove(this MoveComponent self)
        {
            self.BeginTime = TimeHelper.ClientNow();
            self.StartTime = self.BeginTime;
            self.SetNextTarget();
            self.ShouldMove = true;
        }

        private static void SetNextTarget(this MoveComponent self)
        {
            Unit unit = self.GetParent<Unit>();

            self.NextPointIndex++;

            // 时间计算用服务端的位置, 但是移动要用客户端的位置来插值
            Vector3 v = self.GetFaceV();
            float distance = v.magnitude;

            // 插值的起始点要以unit的真实位置来算
            self.StartPos = unit.Position;

            self.AccumulateTime = 0;
            self.StartTime += self.NeedTime;

            self.NeedTime = (long)(distance / self.Speed * 1000);


            if (self.TurnTime > 0)
            {
                // 要用unit的位置
                Vector3 faceV = self.GetFaceV();
                if (faceV.sqrMagnitude < 0.0001f)
                {
                    return;
                }

                self.From = unit.Rotation;

                if (self.IsTurnHorizontal)
                {
                    faceV.y = 0;
                }

                if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
                {
                    self.To = Quaternion.LookRotation(faceV, Vector3.up);
                }

                return;
            }

            if (self.TurnTime == 0) // turn time == 0 立即转向
            {
                Vector3 faceV = self.GetFaceV();
                if (self.IsTurnHorizontal)
                {
                    faceV.y = 0;
                }

                if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
                {
                    self.To = Quaternion.LookRotation(faceV, Vector3.up);
                    unit.Rotation = self.To;
                }
            }
        }

        /// <summary>
        /// 寻路到某个点然后做某事，如果当前距离小于目标距离则直接进入状态，否则就寻路到适合的地点再进行状态切换
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target">目标地点</param>
        /// <param name="targetRange">目标距离</param>
        /// <param name="targetState">目标状态</param>
        public static async UniTaskVoid NavigateTodoSomething(this Unit self, Vector3 target, float targetRange,
            AFsmStateBase targetState, CancellationToken unitaskCancellationToken = default)
        {
            Unit unit = self;

            if (!unit.GetComponent<StackFsmComponent>().ChangeState<NavigateState>(StateTypes.Run, "Navigate", 1))
            {
                ReferencePool.Release(targetState);
                return;
            }

            if (await unit.FindPathMoveToAsync(target, targetRange, unitaskCancellationToken))
            {
                if (targetState != null)
                {
                    if (unit.GetComponent<StackFsmComponent>().ChangeState(targetState))
                    {
                        Game.EventSystem.Publish(unit, new EventType.FSMStateChanged_PlayAnim());
                    }
                }
            }
        }

        private static Vector3 GetFaceV(this MoveComponent self)
        {
            return self.NextTarget - self.PreTarget;
        }

        public static bool FlashTo(this MoveComponent self, Vector3 target)
        {
            Unit unit = self.GetParent<Unit>();
            unit.Position = target;
            return true;
        }

        public static bool MoveTo(this MoveComponent self, Vector3 target, float speed, int turnTime = 0,
            bool isTurnHorizontal = false)
        {
            if (speed < 0.001)
            {
                Log.Error($"speed is 0 {self.GetParent<Unit>().ConfigId} {self.GetParent<Unit>().Id} {speed}");
                return false;
            }

            self.Stop();

            self.IsTurnHorizontal = isTurnHorizontal;
            self.TurnTime = turnTime;
            self.Targets.Add(self.GetParent<Unit>().Position);
            self.Targets.Add(target);
            self.Speed = speed;

            self.StartMove();
            return true;
        }

        public static bool MoveTo(this MoveComponent self, List<Vector3> target, float speed, int turnTime = 0)
        {
            if (target.Count == 0)
            {
                return true;
            }

            if (Math.Abs(speed) < 0.001)
            {
                Log.Error($"speed is 0 {self.GetParent<Unit>().ConfigId} {self.GetParent<Unit>().Id}");
                return false;
            }

            self.Stop();

            foreach (Vector3 v in target)
            {
                self.Targets.Add(v);
            }

            self.IsTurnHorizontal = true;
            self.TurnTime = turnTime;
            self.Speed = speed;

            self.StartMove();

            return true;
        }

        public static void Stop(this MoveComponent self, bool result = false)
        {
            self.Clear(result);
        }

        public static void Clear(this MoveComponent self, bool result = false)
        {
            self.StartTime = 0;
            self.StartPos = Vector3.zero;
            self.BeginTime = 0;
            self.NeedTime = 0;
            self.AccumulateTime = 0;
            self.Targets.Clear();
            self.Speed = 0;
            self.NextPointIndex = 0;
            self.TurnTime = 0;
            self.IsTurnHorizontal = false;
            self.ShouldMove = false;

            if (self.Callback != null)
            {
                Action<bool> callback = self.Callback;
                self.Callback = null;
                callback.Invoke(result);
            }
        }
    }
}