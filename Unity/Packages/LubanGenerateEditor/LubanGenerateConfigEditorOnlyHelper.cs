// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年7月9日, 星期六
// --------------------------

using System;
using ET.Editor.SkillConfig;
using SimpleJSON;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class LubanGenerateConfigEditorOnlyHelper
    {
        private const string c_PackagePath = "Packages/com.nkg.lubangenerateconfig.editor/GenerateConfigData";

        /// <summary>
        /// 获取单行配置
        /// </summary>
        /// <param name="pathToGenerateConfigData">相对于GenerateConfigData的路径</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetConfig<T>(string pathToGenerateConfigData, int id) where T : Bright.Config.EditorBeanBase
        {
            T config = Activator.CreateInstance<T>();

            TextAsset textAsset =
                AssetDatabase.LoadAssetAtPath<TextAsset>($"{c_PackagePath}/{pathToGenerateConfigData}/{id}.json");
            config.LoadJson(JSON.Parse(textAsset.text).AsObject);

            return config;
        }
        
        /// <summary>
        /// 获取单行配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SkillCanvasConfig GetSkillCanvasConfig(int id)
        {
            SkillCanvasConfig config = new SkillCanvasConfig();

            TextAsset textAsset =
                AssetDatabase.LoadAssetAtPath<TextAsset>($"{c_PackagePath}/SkillConfig.TbSkillCanvas/{id}.json");
            config.LoadJson(JSON.Parse(textAsset.text).AsObject);

            return config;
        }
    }
}