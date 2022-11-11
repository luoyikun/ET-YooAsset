using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ET
{
    /// <summary>
    /// 快捷键打开FGUI编辑器 以及工程
    /// </summary>
    public static class FguiEditorTool
    {
        private static string GetProjPath(string relativePath = "")
        {
            if (relativePath == null)
            {
                relativePath = "";
            }

            relativePath = relativePath.Trim();
            if (!string.IsNullOrEmpty(relativePath))
            {
                if (relativePath.Contains("\\"))
                {
                    relativePath = relativePath.Replace("\\", "/");
                }

                if (!relativePath.StartsWith("/"))
                {
                    relativePath = "/" + relativePath;
                }
            }

            return Application.dataPath + relativePath;
        }

        private static void OpenFileOrFolder(string path)
        {
            Process.Start("explorer.exe", path.Replace("/", "\\"));
        }


        private static void DoBat(string path, string param = null, string openFolder = null)
        {
            try
            {
                if (string.IsNullOrEmpty(param))
                {
                    Process.Start(GetProjPath(path));
                }
                else
                {
                    Process.Start(GetProjPath(path), GetProjPath(param));
                }

                if (openFolder != null)
                {
                    OpenFileOrFolder(GetProjPath(openFolder));
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.ToString());
            }
        }


        [MenuItem("Tools/打开FGUI编辑器 %#_e", false, 0)]
        public static void OpenUiEditor()
        {
            DoBat("../../FGUIProject/FairyGUI-Editor/FairyGUI-Editor.exe");
        }
    }
}