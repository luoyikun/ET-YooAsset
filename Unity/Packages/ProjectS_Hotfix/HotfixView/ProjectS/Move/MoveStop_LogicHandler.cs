// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月16日, 星期六
// --------------------------

using Cysharp.Threading.Tasks;
using ET.EventType;

namespace ET
{
    [Event(SceneType.SingleGame)]
    public class MoveStop_LogicHandler : AEvent<Unit, EventType.MoveStop>
    {
        protected override UniTask Run(Unit entity, MoveStop a)
        {
            entity.GetComponent<StackFsmComponent>().RemoveState(StateTypes.Run);
            return UniTask.CompletedTask;
        }
    }
}