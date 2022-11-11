using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class GameObjectComponent : Entity, IAwake<YooAssetProxy.YooAssetResType, string>, IDestroy
    {
        public YooAssetProxy.YooAssetResType YooAssetResType;
        public string ResName;
        public GameObject GameObject { get; set; }
    }
}