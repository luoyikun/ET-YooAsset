﻿using ET.EventType;

namespace ET
{
    // public class ReplaceAnimBuffSystem_1: AEvent<EventType.ReplaceAnimBuffSystemExcuteEvent>
    // {
    //     protected override async UniTask Run(ReplaceAnimBuffSystemExcuteEvent a)
    //     {
    //         ReplaceAnimBuffData replaceAnimBuffData = a.ReplaceAnimBuffData;
    //         AnimationComponent animationComponent = a.Target.GetComponent<AnimationComponent>();
    //         foreach (var animMapInfo in replaceAnimBuffData.AnimReplaceInfo)
    //         {
    //             a.ReplacedAnimData[animMapInfo.StateType] = animationComponent.RuntimeAnimationClips[animMapInfo.StateType];
    //             animationComponent.RuntimeAnimationClips[animMapInfo.StateType] = animMapInfo.AnimName;
    //         }
    //
    //         animationComponent.PlayAnimByStackFsmCurrent();
    //         await UniTask.CompletedTask;
    //     }
    // }
    //
    // public class ReplaceAnimBuffSystem_11: AEvent<EventType.RepalceAnimBuffSystemFinishEvent>
    // {
    //     protected override async UniTask Run(RepalceAnimBuffSystemFinishEvent a)
    //     {
    //         ReplaceAnimBuffData replaceAnimBuffData = a.ReplaceAnimBuffData;
    //         AnimationComponent animationComponent = a.Target.GetComponent<AnimationComponent>();
    //         foreach (var animMapInfo in a.ReplacedAnimData)
    //         {
    //             animationComponent.RuntimeAnimationClips[animMapInfo.Key] = animMapInfo.Value;
    //         }
    //
    //         animationComponent.PlayAnimByStackFsmCurrent();
    //         a.ReplacedAnimData.Clear();
    //         await UniTask.CompletedTask;
    //     }
    // }
}