using ZMH.Common.CommonHelper;
using ZMH.Common.Exceptions;
using FastMember;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace ZMH.Common
{
    /// <summary>
    /// Common缓存管理
    /// Author: Minghua
    /// </summary>
    public class EcisCommonCacheManager
    {
        //Ecis.Common\CommonHelper\AssemblyCacheHelper.cs
        public static Dictionary<string, Assembly> AssemblyCache = new Dictionary<string, Assembly>();

        public static Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        //Ecis.Common\CommonHelper\PluginCacheHelper.cs
        public static List<PluginData> PluginCache = new List<PluginData>();

        //Ecis.Common\Extension\EnumUtil.cs
        public static Dictionary<EnumTypeName, string> EnumDescriptionCache = new Dictionary<EnumTypeName, string>();

        public static Dictionary<EnumTypeName, string> EnumValueStrCache = new Dictionary<EnumTypeName, string>();

        //Ecis.Common\Extension\DataTableExtension.cs
        public static Dictionary<Type, List<DtColumn>> DtColCache = new Dictionary<Type, List<DtColumn>>();

        public static Dictionary<Type, List<PropertyInfo>> DtPropCache = new Dictionary<Type, List<PropertyInfo>>();

        //Ecis.Common\DBHelper\MSSQLProviderBase.cs
        public static Dictionary<Type, List<string>> tpPropNameCache = new Dictionary<Type, List<string>>();

        public static T GetData<T>(string key, Func<T> cachePopulate, bool forceGet = false)
        {
            ExceptUtil.ThrowIfNullOrEmpty(() => key);
            ExceptUtil.ThrowIfNull(() => cachePopulate);

            if (HttpRuntime.Cache[key] != null && forceGet == false)
            {
                return (T)HttpRuntime.Cache[key];
            }
            else
            {
                var value = cachePopulate();
                if (value != null)
                {
                    if (forceGet)
                    {
                        HttpRuntime.Cache.Remove(key);
                    }

                    HttpRuntime.Cache.Insert(key, value, null,
                        DateTime.UtcNow.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return value;
            }
        }

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void Dispose()
        {
            AssemblyCache.Clear();
            TypeCache.Clear();
            PluginCache.Clear();
            EnumDescriptionCache.Clear();
            EnumValueStrCache.Clear();
            DtColCache.Clear();
            DtPropCache.Clear();
            tpPropNameCache.Clear();

            HttpRuntime.Close();
        }
    }

    /// <summary>
    /// Enum缓存Key结构
    /// </summary>
    public struct EnumTypeName
    {
        public Type EnumType;
        public string Name;
    }

    /// <summary>
    /// DataTable扩展结构
    /// </summary>
    public struct DtColumn
    {
        public string ColumnName;
        public Type ColumnType;
    }
}