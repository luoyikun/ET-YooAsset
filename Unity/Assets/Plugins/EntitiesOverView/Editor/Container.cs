// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年8月28日, 星期日
// --------------------------

using Sirenix.OdinInspector;

namespace ET
{
    public class Container: SerializedScriptableObject
    {
        [HideReferenceObjectPicker]
        public Entity Entity;
    }
}