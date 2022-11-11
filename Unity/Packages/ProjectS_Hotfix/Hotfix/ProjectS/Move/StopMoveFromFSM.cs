using Cysharp.Threading.Tasks;
using ET.EventType;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class StopMoveFromFSM : AEvent<Unit,EventType.CancelMoveFromFSM>
    {
        protected override UniTask Run(Unit entity, CancelMoveFromFSM a)
        {
            entity.Stop();
            return UniTask.CompletedTask;
        }
    }
}