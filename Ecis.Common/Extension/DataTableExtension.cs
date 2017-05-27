using Ecis.Common.Exceptions;
using FastMember;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Ecis.Common.Extension
{
    public static class DataTableExtension
    {
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void Dispose()
        {
            EcisCommonCacheManager.DtColCache.Clear();
            EcisCommonCacheManager.DtPropCache.Clear();
        }

        public static DataTable ToFastDataTable<T>(this IEnumerable<T> list)
        {
            ExceptUtil.ThrowIfNull(() => list);

            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create<T>(list))
            {
                table.Load(reader);
            }
            return table;
        }

        public static DataTable ToCacheDataTable<T>(this IEnumerable<T> list)
        {
            ExceptUtil.ThrowIfNull(() => list);

            DataTable dt = new DataTable();

            //获得反射的入口
            Type type = typeof(T);
            List<DtColumn> dcList = new List<DtColumn>();

            if (EcisCommonCacheManager.DtColCache.TryGetValue(type, out dcList))
            {
                foreach (var dc in dcList)
                {
                    dt.Columns.Add(dc.ColumnName, dc.ColumnType);
                }
            }
            else
            {
                //把所有的public属性加入到集合 并添加DataTable的列
                Array.ForEach<PropertyInfo>(type.GetProperties(),
                    p =>
                    {
                        Type colType = p.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dt.Columns.Add(p.Name, colType);
                    });

                EcisCommonCacheManager.DtColCache.Add(
                    type,
                    dt.Columns.Cast<DataColumn>().Select(s => new DtColumn()
                    {
                        ColumnName = s.ColumnName,
                        ColumnType = s.DataType
                    })
                    .ToList());
            }

            //创建属性的集合
            List<PropertyInfo> pList;
            if (EcisCommonCacheManager.DtPropCache.TryGetValue(type, out pList) == false)
            {
                if (pList == null)
                {
                    pList = new List<PropertyInfo>();
                }
                Array.ForEach<PropertyInfo>(type.GetProperties(),
                    p =>
                    {
                        pList.Add(p);
                    });
                EcisCommonCacheManager.DtPropCache.Add(type, pList);
            }

            foreach (var item in list)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p =>
                {
                    var val = p.GetValue(item, null);
                    row[p.Name] = (val == null ? DBNull.Value : val);
                });
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>
        /// 转化一个DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            ExceptUtil.ThrowIfNull(() => list);

            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(T);
            DataTable dt = new DataTable();

            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach<PropertyInfo>(type.GetProperties(),
                p =>
                {
                    pList.Add(p);
                    Type colType = p.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    dt.Columns.Add(p.Name, colType);
                });

            foreach (var item in list)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p =>
                {
                    var val = p.GetValue(item, null);
                    row[p.Name] = (val == null ? DBNull.Value : val);
                });
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static List<T> CacheList<T>(this DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            //var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            List<PropertyInfo> plist;
            if (EcisCommonCacheManager.DtPropCache.TryGetValue(t, out plist) == false)
            {
                if (plist == null)
                {
                    plist = new List<PropertyInfo>(t.GetProperties());
                }
                EcisCommonCacheManager.DtPropCache.Add(t, plist);
            }

            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null && info.CanWrite)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        public static List<T> List<T>(this DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null && info.CanWrite)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表
            Array.ForEach<PropertyInfo>(t.GetProperties(), p =>
            {
                if (dt.Columns.IndexOf(((DescriptionAttribute)Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute))).Description) != -1)
                    prlist.Add(p);
            });
            //创建返回的集合
            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                T ob = new T();

                //找到对应的数据  并赋值
                prlist.ForEach(p =>
                {
                    if (row[((DescriptionAttribute)Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute))).Description] != DBNull.Value && p.CanWrite)
                        p.SetValue(ob, row[((DescriptionAttribute)Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute))).Description], null);
                });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    Type colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(pi.Name, colType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /**/

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable<T>(list, null);
        }

        /// <summary>
        /// 针对泛型集合Display属性转换成的DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDisplayAttrDT<T>(IList<T> list)
        {
            DataTable result = new DataTable();
            if (list != null && list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    var attrObj = Attribute.GetCustomAttribute(pi, typeof(DisplayAttribute));
                    if (attrObj == null) continue;

                    string Description = ((DisplayAttribute)attrObj).GetName();

                    Type colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(Description, colType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        var attrObj = Attribute.GetCustomAttribute(pi, typeof(DisplayAttribute));
                        if (attrObj == null) continue;

                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    var attrObj = Attribute.GetCustomAttribute(pi, typeof(DescriptionAttribute));
                    if (attrObj == null) continue;

                    string Description = ((DescriptionAttribute)attrObj).Description;
                    if (propertyNameList.Count == 0)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        result.Columns.Add(Description, colType);
                        //result.Columns.Add(pi.Name, colType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(Description, pi.PropertyType);
                        //result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// DataTable转化为Json, easyui的table
        /// </summary>
        /// <param name="dt">datatable,dt.TableName 必须为rows</param>
        /// <param name="withTableName">withTableName</param>
        /// <returns>Json</returns>
        public static string DataTable2Json(DataTable dt, bool withTableName, int total)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            if (withTableName)
            {
                jsonBuilder.Append("{\"total\":");
                jsonBuilder.Append(total);
                jsonBuilder.Append(",\"");
                jsonBuilder.Append(dt.TableName);
                jsonBuilder.Append("\":");
            }
            jsonBuilder.Append("[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            if (withTableName) jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// DataTable转化为Json, easyui的table
        /// </summary>
        /// <param name="dt">datatable,dt.TableName 必须为rows</param>
        /// <param name="withTableName">withTableName</param>
        /// <param name="total">总行数</param>
        /// <param name="jsonFooter">页脚的json字符串</param>
        /// <returns>Json</returns>
        public static string DataTable2Json(DataTable dt, bool withTableName, int total, string jsonFooter)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            jsonBuilder.Append("{\"total\":");
            jsonBuilder.Append(total);
            jsonBuilder.Append(",\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":");

            jsonBuilder.Append("[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append(jsonFooter);
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns></returns>
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Copy();
            newdt.Clear();

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }

        public static string ConvertDataTableToXML(DataTable dt)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new XmlTextWriter(stream, Encoding.UTF8))
                    {
                        dt.WriteXml(writer);
                        int count = (int)stream.Length;
                        byte[] arr = new byte[count];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(arr, 0, count);
                        UTF8Encoding utf = new UTF8Encoding();
                        return utf.GetString(arr).Trim();
                    }
                }
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}