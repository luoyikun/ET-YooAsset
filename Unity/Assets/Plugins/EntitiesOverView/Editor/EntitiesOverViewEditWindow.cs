// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年8月28日, 星期日
// --------------------------

using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class EntitiesOverViewEditWindow : OdinMenuEditorWindow
    {
        private PropertyTree m_PropertyTreeForDraw;
        private Container m_Container;

        [MonKey.Command("Open EntitiesOverViewWindow", "Entities层级查看工具", Category = "Debug")]
        public static void ShowWindow()
        {
            EntitiesOverViewEditWindow window = EditorWindow.GetWindow<EntitiesOverViewEditWindow>();

            window.MenuWidth = 250f;
            window.position = GUIHelper.GetEditorWindowRect().AlignCenterXY(850f, 700f);
        }

        protected override void OnGUI()
        {
            GUILayout.BeginHorizontal();

            GUI.color = Color.HSVToRGB(
                Mathf.Cos((float)UnityEditor.EditorApplication.timeSinceStartup + 1f) * 0.125f + 0.325f, 1, 1);
            if (GUILayout.Button(
                    new GUIContent("强制更新"), GUILayout.Width(200)))
            {
                ForceMenuTreeRebuild();
            }

            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            base.OnGUI();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree odinMenuTree = new OdinMenuTree();
            odinMenuTree.Selection.SupportsMultiSelect = false;
            odinMenuTree.Selection.SelectionChanged += this.SelectionChanged;
            odinMenuTree.Config.DrawSearchToolbar = true;
            odinMenuTree.Config.DefaultMenuStyle.Height = 22;

            if (Application.isPlaying)
            {
                EntitiesOverViewUtils.BuildMenuTree(odinMenuTree);
            }
            
            return odinMenuTree;
        }

        private void SelectionChanged(SelectionChangedType obj)
        {
            if (base.MenuTree.Selection.SelectedValue is Entity entity)
            {
                if (m_Container == null)
                {
                    m_Container = ScriptableObject.CreateInstance<Container>();
                }

                m_Container.Entity = entity;
                this.m_PropertyTreeForDraw = PropertyTree.Create(m_Container);
            }
        }

        protected override void DrawEditors()
        {
            if (this.m_PropertyTreeForDraw != null)
            {
                m_PropertyTreeForDraw.Draw(false);
            }
        }
    }
}