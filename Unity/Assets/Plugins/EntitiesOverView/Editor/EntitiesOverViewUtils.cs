// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年8月28日, 星期日
// --------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;

namespace ET
{
    public static class EntitiesOverViewUtils
    {
        public static void BuildMenuTree(OdinMenuTree odinMenuTree)
        {
            if (Game.Scene == null)
            {
                return;
            }

            // ValueTuple Item1 : 路径
            // ValueTuple Item2 : 名字
            List<((string, string), Entity)> allEntitiesHierarchy = new List<((string, string), Entity)>();
            BuildEntitiesHierarchyRecursivly(Game.Scene, string.Empty, allEntitiesHierarchy, string.Empty);

            foreach (var entity in allEntitiesHierarchy)
            {
                OdinMenuItem odinMenuItem = new OdinMenuItem(odinMenuTree, entity.Item1.Item2, entity.Item2)
                {
                    SearchString = entity.Item1.Item1,
                };
                odinMenuTree.AddMenuItemAtPath(entity.Item1.Item1, odinMenuItem);
            }
            
            odinMenuTree.MarkDirty();
        }

        private static void BuildEntitiesHierarchyRecursivly(Entity entity, string parentCategory,
            in List<((string, string), Entity)> result, string prefix = "")
        {
            BuildSingleEntityHierarchy(entity, parentCategory, result, prefix);
        }

        private static void BuildSingleEntityHierarchy(Entity entity, string parentCategory,
            in List<((string, string), Entity)> result, string prefix = "")
        {
            Type type = entity.GetType();
            string itemName = String.Empty;
            foreach (var fieldInfo in UtilityRefelection.GetFieldInfos(type))
            {
                if (UtilityAttribute.TryGetFieldInfoAttribute<LabelTextInEntitiesOverViewAttribute>(fieldInfo,
                        out var attribute))
                {
                    itemName = fieldInfo.GetValue(entity) as string;
                }
            }

            if (string.IsNullOrEmpty(itemName))
            {
                foreach (var propertyInfo in UtilityRefelection.GetPropertyInfos(type))
                {
                    if (UtilityAttribute.TryGetPropertyInfoAttribute<LabelTextInEntitiesOverViewAttribute>(
                            propertyInfo,
                            out var attribute))
                    {
                        itemName = propertyInfo.GetValue(entity) as string;
                    }
                }
            }

            if (string.IsNullOrEmpty(itemName))
            {
                itemName = type.ToString();
            }

            string itemPath = string.IsNullOrEmpty(parentCategory)
                ? string.IsNullOrEmpty(prefix) ? $"{itemName}" : $"{prefix}/{itemName}"
                : string.IsNullOrEmpty(prefix)
                    ? $"{parentCategory}"
                    : $"{parentCategory}/{prefix}";

            result.Add(((itemPath, itemName), entity));

            string childCategroup = string.IsNullOrEmpty(prefix) ? $"{itemPath}" : $"{itemPath}/{itemName}";
            BuildInternalEntitiesHierarchyRecursivly("拥有的子Entity", entity.Children.Values.ToList(), childCategroup,
                result);
            BuildInternalEntitiesHierarchyRecursivly("拥有的Components", entity.Components.Values.ToList(), childCategroup,
                result);
        }
        
        private static void BuildInternalEntitiesHierarchyRecursivly(string prefix, List<Entity> target,
            string parentCategory,
            in List<((string, string), Entity)> result)
        {
            foreach (var child in target)
            {
                BuildSingleEntityHierarchy(child, parentCategory, result, prefix);
            }
        }
    }
}