// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月25日, 星期一
// --------------------------

using UnityEngine;

namespace ET
{
    #region System

    [ObjectSystem]
    public class MouseTargetSelectorComponentComponentAwakeSystem : AwakeSystem<MouseTargetSelectorComponent>
    {
        public override void Awake(MouseTargetSelectorComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class MouseTargetSelectorComponentComponentUpdateSystem : UpdateSystem<MouseTargetSelectorComponent>
    {
        public override void Update(MouseTargetSelectorComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class
        MouseTargetSelectorComponentComponentFixedUpdateSystem : FixedUpdateSystem<MouseTargetSelectorComponent>
    {
        public override void FixedUpdate(MouseTargetSelectorComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class MouseTargetSelectorComponentComponentDestroySystem : DestroySystem<MouseTargetSelectorComponent>
    {
        public override void Destroy(MouseTargetSelectorComponent self)
        {
            self.Destroy();
        }
    }

    #endregion


    public static class MouseTargetSelectorComponent_Utilities
    {
        #region 生命周期函数

        public static void Awake(this MouseTargetSelectorComponent self)
        {
            //此处填写Awake逻辑
            self.m_MainCamera = Camera.main;
            self.m_TargetLayerInfo = LayerMask.GetMask("Map", "Unit");
        }

        public static void Update(this MouseTargetSelectorComponent self)
        {
            self.ResetTargetInfo();

            //此处填写Update逻辑
            if (Physics.Raycast(
                    self.m_MainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 1000,
                    self.m_TargetLayerInfo))
            {
                UnitComponent unitComponent = self.DomainScene().GetComponent<UnitComponent>();
                self.TargetHitPoint = hitInfo.point;
                self.TargetGameObject = hitInfo.transform.gameObject;

                MonoBridge monoBridge = hitInfo.transform.GetComponent<MonoBridge>();
                if (monoBridge == null)
                {
                    return;
                }

                Unit unit = unitComponent.Get(monoBridge.BelongToUnitId);
                if (unit != null)
                {
                    self.TargetUnit = unit;
                }
            }
        }

        public static void FixedUpdate(this MouseTargetSelectorComponent self)
        {
            //此处填写FixedUpdate逻辑
        }

        public static void Destroy(this MouseTargetSelectorComponent self)
        {
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            self.m_MainCamera = null;
            self.ResetTargetInfo();
        }

        #endregion
    }
}