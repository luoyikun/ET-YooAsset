// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月27日, 星期三
// --------------------------

using UnityEditor.SceneManagement;
using UnityEngine;
using YooAsset.Editor;

namespace ET
{
    public class BuildSVC
    {
        [MonKey.Command("Build SVC", "ProjectS编辑器下Build SVC工具", Category = "Build")]
        public static void BuildSVC_Auto()
        {
            ShaderVariantCollector.Run(ShaderVariantCollectorSettingData.Setting.SavePath, () =>
            {
                EditorSceneManager.OpenScene(GlobalDefine.InitScenePath);
            });
        }
    }
}