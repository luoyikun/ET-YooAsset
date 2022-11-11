// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月23日, 星期六
// --------------------------

using YooAsset.Editor;

namespace ET
{
    public class PackShaderVariants : IPackRule
    {
        public string GetBundleName(PackRuleData data)
        {
            return "projects_shaders";
        }
    }
}