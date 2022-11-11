using Cysharp.Threading.Tasks;
using ET.EventType;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class Event_PlayRunAnimationByMoveSpeed: AEvent<Unit ,EventType.MoveStart>
    {
        protected override UniTask Run(Unit entity, MoveStart a)
        {
            if (entity.GetComponent<StackFsmComponent>().GetCurrentFsmState().StateTypes != StateTypes.Run)
            {
                return UniTask.CompletedTask;
            }

            NumericComponent numericComponent = entity.GetComponent<NumericComponent>();
            float animSpeed = numericComponent[NumericType.MoveSpeed] / numericComponent[NumericType.MoveSpeed];
            entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent(0.3f, animSpeed);
            
            return UniTask.CompletedTask;
        }
    }
}