using Cysharp.Threading.Tasks;
using ET.EventType;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class StackFSMComponentSystem_StateChanged_View_PlayAnim: AEvent<Unit,EventType.FSMStateChanged_PlayAnim>
    {
        protected override UniTask Run(Unit entity, FSMStateChanged_PlayAnim a)
        {
            entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
            return UniTask.CompletedTask;
        }
    }
}