using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Web.Services.Description;

namespace Ecis.Common.CommonHelper
{
    /// <summary>
    /// Cache helper class for Assembly and Type
    /// Author: Minghua
    /// </summary>
    public class AssemblyCacheHelper
    {
        private static string _executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static Assembly GetAssembly(string dll)
        {
            string assemblyName = Path.Combine(_executingLocation, dll);

            Assembly asm = null;
            if (EcisCommonCacheManager.AssemblyCache.TryGetValue(assemblyName, out asm))
            {
                return asm;
            }
            else
            {
                try
                {
                    if (File.Exists(assemblyName))
                    {
                        asm = Assembly.LoadFile(assemblyName);
                    }
                    else
                    {
                        asm = Assembly.Load(dll);
                    }
                    if (asm != null)
                    {
                        EcisCommonCacheManager.AssemblyCache.Add(assemblyName, asm);
                        PreJitControls(asm);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return asm;
        }

        public static Type GetType(Assembly asm, string typename)
        {
            if (asm == null)
            {
                return null;
            }

            Type t = null;
            if (EcisCommonCacheManager.TypeCache.TryGetValue(typename, out t)) //比ContainKey性能好很多
            {
                return t;
            }
            else
            {
                t = asm.GetType(typename, false);
                EcisCommonCacheManager.TypeCache.Add(typename, t);
            }
            return t;
        }

        /// <summary>
        /// 根据Web服务地址等参数返回程序集
        /// </summary>
        /// <param name="url"></param>
        /// <param name="nsClassName"></param>
        /// <param name="cdt"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyByWSUrl(
            string url,
            string nsClassName = "",
            NetworkCredential cdt = null)
        {
            Assembly assembly;
            string upperURL = url.ToUpper();
            if (EcisCommonCacheManager.AssemblyCache.ContainsKey(upperURL))
            {
                assembly = (Assembly)EcisCommonCacheManager.AssemblyCache[upperURL];
            }
            else
            {
                int li = nsClassName.LastIndexOf('.');
                string @namespace = (li == -1 ? "" : nsClassName.Substring(0, li));
                using (WebClient wc = new WebClient())
                {
                    if (cdt != null)
                    {
                        wc.Credentials = cdt;
                    }

                    using (Stream stream = wc.OpenRead(url + "?wsdl"))
                    {
                        ServiceDescription sd = ServiceDescription.Read(stream);
                        ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                        sdi.AddServiceDescription(sd, "", "");
                        CodeNamespace cn = new CodeNamespace(@namespace);
                        CodeCompileUnit ccu = new CodeCompileUnit();
                        ccu.Namespaces.Add(cn);
                        sdi.Import(cn, ccu);

                        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                        CompilerParameters cplist = new CompilerParameters();
                        cplist.GenerateExecutable = false;
                        //是否在内存中生成输出
                        cplist.GenerateInMemory = true;

                        cplist.ReferencedAssemblies.Add("System.dll");
                        cplist.ReferencedAssemblies.Add("System.XML.dll");
                        cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                        cplist.ReferencedAssemblies.Add("System.Data.dll");
                        CompilerResults cr = provider.CompileAssemblyFromDom(cplist, ccu);
                        if (true == cr.Errors.HasErrors)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (CompilerError ce in cr.Errors)
                            {
                                sb.Append(ce.ToString());
                                sb.Append(System.Environment.NewLine);
                            }
                            throw new Exception(sb.ToString());
                        }
                        assembly = cr.CompiledAssembly;
                        EcisCommonCacheManager.AssemblyCache[upperURL] = assembly;
                    }
                }
            }
            return assembly;
        }

        /// <summary>
        /// 手动即时编译
        /// </summary>
        /// <param name="type"></param>
        private static void PreJitMarkedMethods(Type type)
        {
            // get the type of all the methods within this instance
            var methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Public |
                                        BindingFlags.Instance |
                                        BindingFlags.Static);

            foreach (var method in methods)
            {
                //手动即时编译 jitting of the method happends here.
                RuntimeHelpers.PrepareMethod(method.MethodHandle);
            }
        }

        /// <summary>
        /// 程序集预即时编译
        /// </summary>
        /// <param name="asm"></param>
        private static void PreJitControls(Assembly asm)
        {
            ThreadPool.QueueUserWorkItem((p) =>
            {
                Thread.Sleep(1000);
                try
                {
                    var tps = asm.GetTypes();
                    foreach (Type t in tps)
                    {
                        PreJitMarkedMethods(t);
                    }
                }
                catch (Exception) { }
            });
        }

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void Dispose()
        {
            EcisCommonCacheManager.AssemblyCache.Clear();
            EcisCommonCacheManager.TypeCache.Clear();
        }
    }
}