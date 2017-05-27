using System;
using System.Linq.Expressions;

namespace Ecis.Common.Extension
{
    public class PropertyHelper
    {
        /// <summary>
        /// requires object instance, but you can skip specifying T
        /// <![CDATA[
        /// 示例:
        /// OrderInfoView oi = new OrderInfoView();
        /// PropertyHelper.GetPropertyName(() => oi.CanEdit)
        /// ]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> exp)
        {
            return (((MemberExpression)(exp.Body)).Member).Name;
        }

        /// <summary>
        /// requires explicit specification of both object type and property type
        /// <![CDATA[
        /// 示例:
        /// PropertyHelper.GetPropertyName<OrderInfoView,bool>(oi => oi.CanEdit)
        /// ]]>
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string GetPropertyName<TObject, TResult>(Expression<Func<TObject, TResult>> exp)
        {
            var me = exp.Body as MemberExpression;
            if (me != null)
            {
                return me.Member.Name;
            }
            return null;
        }

        /// <summary>
        /// requires explicit specification of object type
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string GetPropertyName<TObject>(Expression<Func<TObject, object>> exp)
        {
            return (((MemberExpression)(exp.Body)).Member).Name;
        }
    }
}