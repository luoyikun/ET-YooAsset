//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月15日 13:12:44
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 连接ET和Mono的桥梁
    /// </summary>
    public class MonoBridge : MonoBehaviour
    {
        /// <summary>
        /// 自定义Tag
        /// </summary>
        [LabelText("自定义Tag")] [InfoBox("作用同GameObject.Tag")]
        public string CustomTag;

        /// <summary>
        /// 归属UnitId
        /// </summary>
        public long BelongToUnitId;

        // public Action<MonoBridge> OnCollisionEnter_Callback;
        // public Action<MonoBridge> OnCollisionStay_Callback;
        // public Action<MonoBridge> OnCollisionExit_Callback;

        public Action<MonoBridge> OnTriggerEnter_Callback;
        public Action<MonoBridge> OnTriggerStay_Callback;
        public Action<MonoBridge> OnTriggerExit_Callback;

        // private void OnCollisionEnter(Collision other)
        // {
        //     if (other.gameObject.GetComponent<MonoBridge>() is { } monoBridge)
        //     {
        //         OnCollisionEnter_Callback?.Invoke(monoBridge);
        //     }
        // }
        //
        // private void OnCollisionStay(Collision other)
        // {
        //     if (other.gameObject.GetComponent<MonoBridge>() is { } monoBridge)
        //     {
        //         OnCollisionStay_Callback?.Invoke(monoBridge);
        //     }
        // }
        //
        // private void OnCollisionExit(Collision other)
        // {
        //     if (other.gameObject.GetComponent<MonoBridge>() is { } monoBridge)
        //     {
        //         OnCollisionExit_Callback?.Invoke(monoBridge);
        //     }
        // }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<MonoBridge>() is { } monoBridge)
            {
                OnTriggerEnter_Callback?.Invoke(monoBridge);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<MonoBridge>() is { } monoBridge)
            {
                OnTriggerStay_Callback?.Invoke(monoBridge);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<MonoBridge>() is { } monoBridge)
            {
                OnTriggerExit_Callback?.Invoke(monoBridge);
            }
        }

#if UNITY_EDITOR
        private string m_GameObjectTag;

        [ShowIf("CustomTagNotEqualGameObjectName")]
        [InfoBox("注意，当前自定义Tag与游戏物体名称不一致", InfoMessageType.Warning)]
        [Button("点击此按钮可重置自定义Tag"), GUIColor(0.5f, 0.4f, 0.8f)]
        public void ResetCustomTagEqualGameObjectName()
        {
            Reset();
        }

        public bool CustomTagNotEqualGameObjectName()
        {
            return CustomTag == m_GameObjectTag ? false : true;
        }

        private void Reset()
        {
            CustomTag = this.gameObject.name;
            m_GameObjectTag = this.gameObject.name;
        }
#endif
    }
}