// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年9月21日, 星期三
// --------------------------

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace ET
{
    public class MonkeySettingEditWindow : OdinEditorWindow
    {
        [HideInInspector]
        public MonKeySettings MonKeySettings;

        private PropertyTree _propertyTree;

        [MenuItem("Tools/MonKey Commander/Settings")]
        public static void ShowWindow()
        {
            GetWindow<MonkeySettingEditWindow>().Show();
        }
        
        protected override void OnEnable()
        {
            MonKeySettings = global::MonKeySettings.InitSettings();
            _propertyTree = PropertyTree.Create(MonKeySettings);
        }

        protected override void OnGUI()
        {
            _propertyTree.Draw(false);
        }
    }
}