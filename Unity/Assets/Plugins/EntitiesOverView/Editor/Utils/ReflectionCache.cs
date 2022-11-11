// --------------------------
// 作者：烟雨迷离半世殇
// 邮箱：1778139321@qq.com
// 日期：2022年8月28日, 星期日
// --------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET
{
    public static partial class UtilityRefelection
    {
        static readonly Dictionary<string, Assembly> AssemblyCache = new Dictionary<string, Assembly>();
        static readonly Dictionary<string, Type> FullNameTypeCache = new Dictionary<string, Type>();
        static readonly List<Type> AllTypeCache = new List<Type>();

        static readonly Dictionary<Type, IEnumerable<Type>> ChildrenTypeCache =
            new Dictionary<Type, IEnumerable<Type>>();

        static UtilityRefelection()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Unity")) continue;
                if (!assembly.FullName.Contains("Version=0.0.0")) continue;
                AllTypeCache.AddRange(assembly.GetTypes());
            }
        }

        public static List<Type> GetAllTypes()
        {
            return AllTypeCache;
        }

        public static IEnumerable<Type> GetChildrenTypes<T>()
        {
            return GetChildrenTypes(typeof(T));
        }

        public static IEnumerable<Type> GetChildrenTypes(Type baseType)
        {
            if (ChildrenTypeCache.TryGetValue(baseType, out IEnumerable<Type> childrenTypes))
            {
                foreach (var item in childrenTypes)
                {
                    yield return item;
                }

                yield break;
            }

            ChildrenTypeCache[baseType] = childrenTypes = BuildCache(baseType);
            foreach (var type in childrenTypes)
            {
                yield return type;
            }
        }

        private static IEnumerable<Type> BuildCache(Type baseType)
        {
            foreach (var type in AllTypeCache)
            {
                if (baseType.IsAssignableFrom(type))
                    yield return type;
            }
            //var selfAssembly = Assembly.GetAssembly(_baseType);
            //if (selfAssembly.FullName.StartsWith("Assembly-CSharp") && !selfAssembly.FullName.Contains("-firstpass"))
            //{
            //    foreach (var type in selfAssembly.GetTypes())
            //    {
            //        if (!type.IsAbstract && _baseType.IsAssignableFrom(type))
            //        {
            //            yield return type;
            //        }
            //    }
            //}
            //else
            //{
            //    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //    foreach (Assembly assembly in assemblies)
            //    {
            //        if (assembly.FullName.StartsWith("Unity")) continue;
            //        if (!assembly.FullName.Contains("Version=0.0.0")) continue;
            //        foreach (var type in assembly.GetTypes())
            //        {
            //            if (type != null && !type.IsAbstract && _baseType.IsAssignableFrom(type))
            //                yield return type;
            //        }
            //    }
            //}
        }

        public static Assembly LoadAssembly(string assemblyString)
        {
            Assembly assembly;
            if (!AssemblyCache.TryGetValue(assemblyString, out assembly))
                AssemblyCache[assemblyString] = assembly = Assembly.Load(assemblyString);
            return assembly;
        }

        public static Type GetType(string fullName, string assemblyString)
        {
            Type type;
            if (FullNameTypeCache.TryGetValue(fullName, out type))
                return type;
            Assembly assembly = LoadAssembly(assemblyString);
            if (assembly == null) return null;
            foreach (var tempType in assembly.GetTypes())
            {
                FullNameTypeCache[tempType.FullName] = tempType;
            }

            if (FullNameTypeCache.TryGetValue(fullName, out type))
                return type;
            return null;
        }

        #region GetMemberInfo

        static Dictionary<Type, List<FieldInfo>> _typeFieldInfoCache = new Dictionary<Type, List<FieldInfo>>();

        static Dictionary<Type, List<PropertyInfo>> _typePropertyInfoInfoCache =
            new Dictionary<Type, List<PropertyInfo>>();

        /// <summary> 获取字段，包括基类的私有字段 </summary>
        public static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            return GetFieldInfos(type).Find(f => f.Name == fieldName);
        }

        public static List<FieldInfo> GetFieldInfos(Type type)
        {
            if (_typeFieldInfoCache.TryGetValue(type, out List<FieldInfo> fieldInfos))
                return fieldInfos;
            _typeFieldInfoCache[type] = fieldInfos = new List<FieldInfo>(type.GetFields(BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            // 获取类包含的所有字段(包含私有)
            while ((type = type.BaseType) != null)
            {
                fieldInfos.InsertRange(0,
                    type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                   BindingFlags.DeclaredOnly));
            }

            return fieldInfos;
        }

        public static IEnumerable<FieldInfo> GetFieldInfos(Type type, Func<FieldInfo, bool> patern)
        {
            foreach (var field in GetFieldInfos(type))
            {
                if (patern(field))
                    yield return field;
            }
        }

        /// <summary> 获取字段，包括基类的私有字段 </summary>
        public static PropertyInfo GetPropertyInfo(Type type, string fieldName)
        {
            return GetPropertyInfos(type).Find(f => f.Name == fieldName);
        }

        public static List<PropertyInfo> GetPropertyInfos(Type type)
        {
            if (_typePropertyInfoInfoCache.TryGetValue(type, out List<PropertyInfo> fieldInfos))
                return fieldInfos;
            _typePropertyInfoInfoCache[type] = fieldInfos = new List<PropertyInfo>(
                type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                   BindingFlags.DeclaredOnly));
            // 获取类包含的所有字段(包含私有)
            while ((type = type.BaseType) != null)
            {
                fieldInfos.InsertRange(0,
                    type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly));
            }

            return fieldInfos;
        }

        public static IEnumerable<PropertyInfo> GetPropertyInfos(Type type, Func<PropertyInfo, bool> patern)
        {
            foreach (var field in GetPropertyInfos(type))
            {
                if (patern(field))
                    yield return field;
            }
        }

        /// <summary> 获取方法，包括基类的私有方法 </summary>
        public static MethodInfo GetMethodInfo(Type type, string methodName)
        {
            // 如果第一次没有找到，那么这个变量可能是基类的私有字段
            MethodInfo method = type.GetMethod(methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            // 只搜索基类的私有方法
            while (method == null && (type = type.BaseType) != null)
            {
                method = type.GetMethod(methodName,
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }

            return method;
        }

        #endregion
    }

    public static partial class UtilityAttribute
    {
        #region Class

        /// <summary> 保存类的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Attribute[]> TypeAttributes = new Dictionary<Type, Attribute[]>();

        /// <summary> 尝试获取目标类型的目标特性 </summary>
        public static bool TryGetTypeAttribute<TAttributeType>(Type type, out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            if (TryGetTypeAttributes(type, out Attribute[] attributes))
            {
                foreach (var tempAttribute in attributes)
                {
                    attribute = tempAttribute as TAttributeType;
                    if (attribute != null)
                        return true;
                }
            }

            attribute = null;
            return false;
        }

        /// <summary> 尝试获取目标类型的所有特性 </summary>
        public static bool TryGetTypeAttributes(Type type, out Attribute[] attributes)
        {
            if (TypeAttributes.TryGetValue(type, out attributes))
                return attributes == null || attributes.Length > 0;

            attributes = type.GetCustomAttributes() as Attribute[];
            TypeAttributes[type] = attributes;
            return attributes == null || attributes.Length > 0;
        }

        #endregion

        #region Field

        /// <summary> 保存字段的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Dictionary<string, Attribute[]>> TypeFieldAttributes =
            new Dictionary<Type, Dictionary<string, Attribute[]>>();

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetFieldInfoAttribute<TAttributeType>(FieldInfo fieldInfo,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            attribute = null;
            if (fieldInfo == null) return false;
            if (TryGetFieldInfoAttributes(fieldInfo, out Attribute[] attributes))
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    attribute = attributes[i] as TAttributeType;
                    if (attribute != null)
                        return true;
                }
            }

            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetFieldAttribute<TAttributeType>(Type type, string fieldName,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            return TryGetFieldInfoAttribute(UtilityRefelection.GetFieldInfo(type, fieldName), out attribute);
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetFieldInfoAttributes(FieldInfo fieldInfo,
            out Attribute[] attributes)
        {
            Dictionary<string, Attribute[]> fieldTypes;
            if (TypeFieldAttributes.TryGetValue(fieldInfo.DeclaringType, out fieldTypes))
            {
                if (fieldTypes.TryGetValue(fieldInfo.Name, out attributes))
                {
                    if (attributes != null && attributes.Length > 0)
                        return true;
                    return false;
                }
            }
            else
                fieldTypes = new Dictionary<string, Attribute[]>();

            attributes = fieldInfo.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
            fieldTypes[fieldInfo.Name] = attributes;
            TypeFieldAttributes[fieldInfo.DeclaringType] = fieldTypes;
            if (attributes.Length > 0)
                return true;
            return false;
        }

        #endregion

        #region Property

        /// <summary> 保存字段的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Dictionary<string, Attribute[]>> TypePropertyAttributes =
            new Dictionary<Type, Dictionary<string, Attribute[]>>();

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetPropertyInfoAttribute<TAttributeType>(PropertyInfo fieldInfo,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            attribute = null;
            if (fieldInfo == null) return false;
            if (TryGetPropertyInfoAttributes(fieldInfo, out Attribute[] attributes))
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    attribute = attributes[i] as TAttributeType;
                    if (attribute != null)
                        return true;
                }
            }

            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetPropertyAttribute<TAttributeType>(Type type, string fieldName,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            return TryGetPropertyInfoAttribute(UtilityRefelection.GetPropertyInfo(type, fieldName), out attribute);
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetPropertyInfoAttributes(PropertyInfo fieldInfo,
            out Attribute[] attributes)
        {
            Dictionary<string, Attribute[]> fieldTypes;
            if (TypePropertyAttributes.TryGetValue(fieldInfo.DeclaringType, out fieldTypes))
            {
                if (fieldTypes.TryGetValue(fieldInfo.Name, out attributes))
                {
                    if (attributes != null && attributes.Length > 0)
                        return true;
                    return false;
                }
            }
            else
                fieldTypes = new Dictionary<string, Attribute[]>();

            attributes = fieldInfo.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
            fieldTypes[fieldInfo.Name] = attributes;
            TypePropertyAttributes[fieldInfo.DeclaringType] = fieldTypes;
            if (attributes.Length > 0)
                return true;
            return false;
        }

        #endregion

        #region Method

        /// <summary> 保存方法的特性，在编译时重载 </summary>
        private static readonly Dictionary<Type, Dictionary<string, Attribute[]>> TypeMethodAttributes =
            new Dictionary<Type, Dictionary<string, Attribute[]>>();

        public static bool TryGetMethodInfoAttribute<TAttributeType>(MethodInfo methodInfo,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            if (TryGetMethodInfoAttributes(methodInfo, out Attribute[] attributes))
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    attribute = attributes[i] as TAttributeType;
                    if (attribute != null)
                        return true;
                }
            }

            attribute = null;
            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的目标特性 </summary>
        public static bool TryGetMethodAttribute<TAttributeType>(Type type, string methodName,
            out TAttributeType attribute)
            where TAttributeType : Attribute
        {
            return TryGetMethodInfoAttribute(UtilityRefelection.GetMethodInfo(type, methodName), out attribute);
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetMethodInfoAttributes(MethodInfo methodInfo,
            out Attribute[] attributes)
        {
            Dictionary<string, Attribute[]> methodTypes;
            if (TypeFieldAttributes.TryGetValue(methodInfo.DeclaringType, out methodTypes))
            {
                if (methodTypes.TryGetValue(methodInfo.Name, out attributes))
                {
                    if (attributes != null && attributes.Length > 0)
                        return true;
                    return false;
                }
            }
            else
                methodTypes = new Dictionary<string, Attribute[]>();

            attributes = methodInfo.GetCustomAttributes(typeof(Attribute), true) as Attribute[];
            methodTypes[methodInfo.Name] = attributes;
            TypeFieldAttributes[methodInfo.DeclaringType] = methodTypes;
            if (attributes.Length > 0)
                return true;
            return false;
        }

        /// <summary> 尝试获取目标类型的目标字段的所有特性 </summary>
        public static bool TryGetMethodAttributes(Type type, string methodName,
            out Attribute[] attributes)
        {
            return TryGetMethodInfoAttributes(UtilityRefelection.GetMethodInfo(type, methodName), out attributes);
        }

        #endregion
    }
}