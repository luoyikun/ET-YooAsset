// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年8月28日, 星期日
// --------------------------


using System;
using System.Diagnostics;
using UnityEditor;

namespace ET
{
    /// <summary>
    /// 此Attribute标识的string字段将作为显示在树形列表里的TreeItem Name
    /// 强制要求字段名为 LabelTextInEntitiesOverView_EditorOnly
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    public class LabelTextInEntitiesOverViewAttribute : Attribute
    {
    }
}