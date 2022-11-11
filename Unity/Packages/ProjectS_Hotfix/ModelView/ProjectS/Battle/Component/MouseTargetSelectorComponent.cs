//此文件格式由工具自动生成

using ET.cfg.LuBanSample;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 用于鼠标选择目标的组件，功能类似于UserInputComponent，需要指定目标的其余组件可以从这个组件来获取目标对象
    /// </summary>
    public class MouseTargetSelectorComponent : Entity, IAwake, IUpdate, IFixedUpdate, IDestroy
    {
        #region 私有成员

        public Camera m_MainCamera;

        public int m_TargetLayerInfo;

        /// <summary>
        /// 射线击中Gameobject
        /// </summary>
        public GameObject TargetGameObject;

        /// <summary>
        /// 射线击中Unit
        /// </summary>
        public Unit TargetUnit;

        /// <summary>
        /// 射线击中的点
        /// </summary>
        public Vector3 TargetHitPoint;

        #endregion

        #region 公有成员

        /// <summary>
        /// 重置目标对象数据
        /// </summary>
        public void ResetTargetInfo()
        {
            this.TargetGameObject = null;
            this.TargetUnit = null;
            this.TargetHitPoint = Vector3.zero;
        }

        #endregion
    }
}