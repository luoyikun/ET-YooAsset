using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class LSF_Component : Entity, IAwake, IFixedUpdate, IDestroy
    {
        /// <summary>
        /// 当前帧数
        /// </summary>
        public uint CurrentFrame;
    }
}
