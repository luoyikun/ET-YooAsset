// Administrator20222310:562022

using System.IO;
using UnityEngine;
using YooAsset.Editor;

namespace ET
{
    public class CollectShaderVariants : IFilterRule
    {
        public bool IsCollectAsset(FilterRuleData data)
        {
            return Path.GetExtension(data.AssetPath) == ".shadervariants";		
        }
    }
}