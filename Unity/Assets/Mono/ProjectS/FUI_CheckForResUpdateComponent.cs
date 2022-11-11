//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月26日 22:26:49
//------------------------------------------------------------

namespace ET
{
    public class FUI_CheckForResUpdateComponent
    {
        public static void Init()
        {
            FUIEntry.LoadPackage_MonoOnly("CheckForResUpdate");

            FUI_CheckForResUpdateBinder.BindAll();
            FUI_CheckForResUpdate forResUpdate = FUI_CheckForResUpdate.CreateInstance();
            forResUpdate.MakeFullScreen();
            forResUpdate.m_processbar.max = 1;
            forResUpdate.m_processbar.value = 0;

            FUIManager_MonoOnly.AddUI(nameof(FUI_CheckForResUpdate), forResUpdate);

            YooAssetProxy.InitHostCallbacks((onStateChanged) =>
            {
                forResUpdate.m_updateInfo.text = onStateChanged.CurrentStates.ToString();

                if (onStateChanged.CurrentStates == EPatchStates.PatchDone)
                {
                    forResUpdate.m_updateInfo.text = "资源下载完毕，正在加载核心逻辑。。。";
                }
            }, (onProcessUpdated) =>
            {
                string currentSizeMB = (onProcessUpdated.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
                string totalSizeMB = (onProcessUpdated.TotalDownloadSizeBytes / 1048576f).ToString("f1");
                string text =
                    $"资源下载中：{onProcessUpdated.CurrentDownloadCount}/{onProcessUpdated.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";

                forResUpdate.m_updateInfo.text = text;
                forResUpdate.m_processbar.value = onProcessUpdated.CurrentDownloadSizeBytes * 1.0f /
                    onProcessUpdated.TotalDownloadSizeBytes * 100;
            });
        }

        public static void Release()
        {
            FUIManager_MonoOnly.RemoveUI(nameof(FUI_CheckForResUpdate));
            FUIEntry.RemovePackage_MonoOnly("CheckForResUpdate");
        }
    }
}