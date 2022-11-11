//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 11:09:31
//------------------------------------------------------------

#if !SERVER
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ET
{
    public class MapClickComponentAwake : AwakeSystem<MapClickCompoent>
    {
        public override void Awake(MapClickCompoent self)
        {
            self.m_UserInputComponent = self.DomainScene().GetComponent<UserInputComponent>();

            self.m_MouseTargetSelectorComponent = self.GetParent<Unit>().GetComponent<MouseTargetSelectorComponent>();
        }
    }

    public class MapClickComponentUpdate : UpdateSystem<MapClickCompoent>
    {
        public override void Update(MapClickCompoent self)
        {
            if (self.m_UserInputComponent.RightMouseDown)
            {
                if (self.m_MouseTargetSelectorComponent.TargetGameObject?.GetComponent<MonoBridge>()?.CustomTag ==
                    "Map")
                {
                    self.MapPathFinder(self.m_MouseTargetSelectorComponent.TargetHitPoint).Forget();
                }
            }
        }
    }

    public static class MapClickSystems
    {
        public static async UniTaskVoid MapPathFinder(this MapClickCompoent self, Vector3 ClickPoint)
        {
            Unit unit = self.GetParent<Unit>();
            unit.FindPathMoveToAsync(ClickPoint).Forget();
        }
    }
}
#endif