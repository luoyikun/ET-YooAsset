//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 15:15:19
//------------------------------------------------------------

using System;
using UnityEngine;

namespace ET
{
    public class Effect_Darius_Bleed: MonoBehaviour
    {
        public float RotateSpeed;

        private void Update()
        {
            this.transform.Rotate(0, RotateSpeed * Time.deltaTime, 0, Space.Self);
        }
    }
}