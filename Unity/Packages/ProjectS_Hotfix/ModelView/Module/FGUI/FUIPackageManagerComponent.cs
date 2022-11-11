using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FairyGUI;
using UnityEngine;
using YooAsset;

namespace ET
{
    /// <summary>
    /// 管理所有UI Package
    /// </summary>
    public class FUIPackageManagerComponent : Entity, IAwake, IDestroy
    {
        public YooAssetComponent UsedYooAssetComponent;
    }
}