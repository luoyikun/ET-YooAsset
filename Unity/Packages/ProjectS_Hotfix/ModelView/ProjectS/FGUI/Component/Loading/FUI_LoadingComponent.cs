// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月11日, 星期一
// --------------------------

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using YooAsset;

namespace ET
{
    public class FUI_LoadingComponent : Entity, IAwake<FUI_Loading>,IUpdate, IDestroy
    {
        public FUI_Loading FuiLoading;

        public UniTask<UnityEngine.SceneManagement.Scene> LoadSceneTask; 
        public List<UniTask<UnityEngine.Object>> ResLoadTask = new List<UniTask<UnityEngine.Object>>();

        public int TotalResCountToBeLoaded;
        public int HasLoadCompletedResCount;

        public UniTaskCompletionSource Tcs;
    }
}
