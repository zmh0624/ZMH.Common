using Microsoft.Practices.Unity.InterceptionExtension;
using System.Collections.Generic;

namespace ZMH.Common.Interception
{
    /// <summary>
    /// 错误日志拦截
    /// </summary>
    public class ExceptionLoggingBehavior : InterceptionBase
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            using (new CodeTimer(input.MethodBase.DeclaringType.FullName + "." + input.MethodBase.Name))
            {
                IMethodReturn rt = getNext.Invoke().Invoke(input, getNext);
                if (rt.Exception != null)
                {
                    List<string> args = new List<string>();
                    for (int i = 0; i < input.Arguments.Count; i++)
                    {
                        args.Add(input.Arguments[i].ToString());
                    }

                    //日志打印
                    LogRepository.Log.Error("AOP ERROR: {0}-{1}={2}={3}({4})",
                            rt.Exception.Message,
                            input.MethodBase.DeclaringType.FullName,
                            input.Target.ToString(),
                            input.MethodBase.Name.ToString(),
                            string.Join(",", args)
                            );
                    LogRepository.Log.Error(rt.Exception);
                }
                return rt;
            }
        }
    }
}