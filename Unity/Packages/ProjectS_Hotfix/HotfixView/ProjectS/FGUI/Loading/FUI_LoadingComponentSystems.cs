// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月11日, 星期一
// --------------------------

using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ET
{
    public class FUI_LoadingComponentSystems : AwakeSystem<FUI_LoadingComponent,FUI_Loading>
    {
        public override void Awake(FUI_LoadingComponent self, FUI_Loading a)
        {
            self.FuiLoading = a;
            a.MakeFullScreen();
        }
    }
    
    public class FUI_LoadingComponentUpdateSystems : UpdateSystem<FUI_LoadingComponent>
    {
        public override void Update(FUI_LoadingComponent self)
        {
            for (int i = self.ResLoadTask.Count - 1; i >=0 ; i--)
            {
                if (self.ResLoadTask[i].Status == UniTaskStatus.Succeeded)
                {
                    self.HasLoadCompletedResCount++;
                    self.ResLoadTask.RemoveAt(i);

                    self.FuiLoading.m_Pro_Load.m_Pro_Load.self.value =
                        self.HasLoadCompletedResCount * 1.0f / self.TotalResCountToBeLoaded;
                }
            }

            if (self.ResLoadTask.Count == 0)
            {
                if (self.LoadSceneTask.Status == UniTaskStatus.Succeeded)
                {
                    self.HasLoadCompletedResCount++;
                    self.FuiLoading.m_Pro_Load.m_Pro_Load.self.value =
                        self.HasLoadCompletedResCount * 1.0f / self.TotalResCountToBeLoaded;
                    
                    self.Hide();
                    self.Reset();

                    self.Tcs.TrySetResult();
                }
            }
        }
    }

    public static class FUI_LoadingComponentUtitlites
    {
        public static async UniTask LoadRes(this FUI_LoadingComponent self, string sceneName,List<string> resList)
        {
            self.ResLoadTask.Clear();
            YooAssetComponent yooAssetComponent = self.DomainScene().GetComponent<YooAssetComponent>();

            foreach (var res in resList)
            {
                self.ResLoadTask.Add(yooAssetComponent.LoadAssetAsync<UnityEngine.Object>(res));
            }
            
            self.TotalResCountToBeLoaded = self.ResLoadTask.Count;

            if (!string.IsNullOrEmpty(sceneName))
            {
                self.LoadSceneTask = yooAssetComponent.LoadSceneAsync(sceneName);
                self.TotalResCountToBeLoaded++;
            }

            self.Tcs = new UniTaskCompletionSource();

            await self.Tcs.Task;
        }

        public static void Reset(this FUI_LoadingComponent self)
        {
            self.FuiLoading.m_Pro_Load.m_Txt_LoadInfo.text = string.Empty;
            self.FuiLoading.m_Pro_Load.m_Txt_LoadTip.text = string.Empty;
            self.FuiLoading.m_Pro_Load.m_Pro_Load.self.value = 0;
            self.FuiLoading.m_Pro_Load.m_Pro_Load.self.max = 1;
            self.ResLoadTask.Clear();
            self.LoadSceneTask = default;
        }
        
        public static void Show(this FUI_LoadingComponent self)
        {
            self.FuiLoading.self.visible = true;
        }
        
        public static void Hide(this FUI_LoadingComponent self)
        {
            self.FuiLoading.self.visible = false;
        }
    }
}