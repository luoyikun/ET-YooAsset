using System.IO;
using UnityEditor;

namespace ET
{
    public static class WebFileHelper
    {
        [MonKey.Command("WebFileServer", "本地资源服务器", Category = "Build")]
        public static void OpenFileServer()
        {
            ProcessHelper.Run("dotnet", "FileServer.dll", "../FileServer/");
        }
    }
}