using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Ecis.Common.Extension
{
    /// <summary>
    /// List and Enumerable Extension
    /// Author: Minghua
    /// </summary>
    public static class ListEx
    {
        public static bool HasItems<T>(this T[] array)
        {
            return (array != null) && (array.Length > 0);
        }

        /// <summary>
        /// 是否包含数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool HasItems<T>(this List<T> list)
        {
            return (list != null) && (list.Count > 0);
        }

        /// <summary>
        /// 基本上和List<T>的ForEach方法一致。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col"></param>
        /// <param name="handler"></param>
        public static void Each<T>(this IEnumerable<T> col, Action<T> handler)
        {
            foreach (var item in col)
            {
                handler(item);
            }
        }

        /// <summary>
        /// 带索引的遍历方法。
        /// </summary>
        public static void Each<T>(this IEnumerable<T> col, Action<T, int> handler)
        {
            int index = 0;
            foreach (var item in col)
            {
                handler(item, index++);
            }
        }

        /// <summary>
        /// 可以半途中断执行的遍历方法。
        /// </summary>
        public static void Each<T>(this IEnumerable<T> col, Func<T, bool> handler)
        {
            foreach (var item in col)
            {
                if (!handler(item)) break;
            }
        }

        /// <summary>
        /// 可以半途中段的带索引的遍历方法。
        /// </summary>
        public static void Each<T>(this IEnumerable<T> col, Func<T, int, bool> handler)
        {
            int index = 0;
            foreach (var item in col)
            {
                if (!handler(item, index++)) break;
            }
        }

        public static void Each<T>(this IEnumerable col, Action<object> handler)
        {
            foreach (var item in col)
            {
                handler(item);
            }
        }

        public static void Each<T>(this IEnumerable col, Action<object, int> handler)
        {
            int index = 0;
            foreach (var item in col)
            {
                handler(item, index++);
            }
        }

        public static void Each<T>(this IEnumerable col, Func<object, bool> handler)
        {
            foreach (var item in col)
            {
                if (!handler(item)) break;
            }
        }

        public static void Each<T>(this IEnumerable col, Func<object, int, bool> handler)
        {
            int index = 0;
            foreach (var item in col)
            {
                if (!handler(item, index++)) break;
            }
        }

        /*
         using (IDataReader reader = ...)
        {
            List<Customer> customers = reader.Select(r => new Customer {
                CustomerId = r["id"] is DBNull ? null : r["id"].ToString(),
                CustomerName = r["name"] is DBNull ? null : r["name"].ToString()
            }).ToList();
        }
         */

        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }
}