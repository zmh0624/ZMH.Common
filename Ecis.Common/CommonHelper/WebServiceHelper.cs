using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Services.Protocols;

namespace Ecis.Common.CommonHelper
{
    /// <summary>
    /// 动态生成WebService程序集,并可调用
    /// Author: Minghua
    /// </summary>
    public class WebServiceHelper
    {
        /// <summary>
        /// 传入Cookie，使对方可以使用当前Session
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        private static void SetCookie(string url, SoapHttpClientProtocol obj)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null)
            {
                CookieContainer cc = new CookieContainer();
                foreach (string cookieName in ctx.Request.Cookies.AllKeys)
                {
                    cc.SetCookies(new Uri(url), cookieName + "=" + ctx.Request.Cookies[cookieName].Value);
                }
                obj.CookieContainer = cc;
            }
        }

        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="nsClassName">命名空间</param>
        /// <param name="methodname">方法名 </param>
        /// <param name="cdt">跨服务器时所使用的用户身份，如在编译环境下 设置为null即可</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object Invoke(
            string url,
            string nsClassName,
            string methodname,
            NetworkCredential cdt = null,
            params object[] args)
        {
            if (args != null && args.Length == 1 && args[0] is ArrayList)
            {
                args = (args[0] as ArrayList).ToArray();
            }
            try
            {
                Assembly assembly = AssemblyCacheHelper.GetAssemblyByWSUrl(url, nsClassName, cdt);

                Type t = null;
                if (String.IsNullOrEmpty(nsClassName))
                {
                    t = assembly.GetTypes()[0];
                }
                else
                {
                    t = assembly.GetType(nsClassName, true, true);
                }
                MethodInfo mi = null;
                if (String.IsNullOrEmpty(methodname))
                {
                    mi = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)[0];
                }
                else
                {
                    mi = t.GetMethod(methodname);
                }
                SoapHttpClientProtocol obj = Activator.CreateInstance(t) as SoapHttpClientProtocol;
                SetCookie(url, obj);
                obj.Timeout = 100000;
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new TimeoutException(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        /// <summary>
        /// 用于对象的序列化（对象必须声明[Serializable]）
        /// WebService支持序列化后的byte[]作为参数
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static byte[] SerializeObject(object o)
        {
            if (o == null)
            {
                return null;
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, o);
                memoryStream.Position = 0;
                byte[] read = new byte[memoryStream.Length];
                memoryStream.Read(read, 0, read.Length);
                memoryStream.Close();
                return read;
            }
        }

        /// <summary>
        /// 反序列化对象（对象必须声明[Serializable]）
        /// WebService支持序列化后的byte[]作为参数
        /// </summary>
        /// <param name="bts"></param>
        /// <returns></returns>
        public static object DeserializeObject(byte[] bts)
        {
            object o = null;
            if (bts == null)
            {
                return o;
            }
            using (MemoryStream memoryStream = new MemoryStream(bts))
            {
                memoryStream.Position = 0;
                BinaryFormatter formatter = new BinaryFormatter();
                o = formatter.Deserialize(memoryStream);
                memoryStream.Close();
                return o;
            }
        }
    }
}