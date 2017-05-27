using Ecis.Common.Exceptions;
using NLog;
using System.Linq;

namespace Ecis.Common
{
    /// <summary>
    /// 日志实例
    /// </summary>
    public class LogRepository
    {
        public static ILogger Log
        {
            get
            {
                return LogExcept.Log;
            }
        }

        /// <summary>
        /// 日志写DB([ECISPlatform_4.1Log].[dbo].[NLog])
        /// </summary>
        /// <param name="PVID">患者唯一标识</param>
        /// <param name="moduleName">业务模块名称</param>
        /// <param name="moduleCode">业务模块代码</param>
        /// <param name="clientSite">站点信息,客户端信息,可输入IP</param>
        /// <param name="originalValue">对于信息修改类型日志,修改前值</param>
        /// <param name="content">日志内容,对于修改类型日志,为修改后值</param>
        /// <param name="operationType">操作类型枚举 None 0,Add 1,Modify 2,Delete 3,Query 4,Normal 5</param>
        /// <param name="oper">操作人</param>
        /// <param name="result">操作结果 1 成功,0 失败</param>
        public static void WriteDB(
              string PVID,
              string moduleName,
              string moduleCode,
              string clientSite,
              string originalValue,
              string content,
              int operationType,
              string oper,
              bool result)
        {
            var dbRule = LogManager.Configuration.LoggingRules
                .FirstOrDefault(r =>
                    r.Targets.FirstOrDefault() != null && r.Targets.FirstOrDefault().Name == "db");

            LogLevel lLevel = LogLevel.Error;
            if (dbRule != null)
            {
                lLevel = dbRule.Levels.FirstOrDefault() ?? LogLevel.Error;
            }
            LogEventInfo li = new LogEventInfo(lLevel, Log.Name, content);
            li.Properties["PVID"] = PVID;
            li.Properties["ModuleName"] = moduleName;
            li.Properties["ModuleCode"] = moduleCode;
            li.Properties["ClientSite"] = clientSite;
            li.Properties["OriginalValue"] = originalValue;
            li.Properties["OperationType"] = operationType;
            li.Properties["Operator"] = oper;
            li.Properties["Result"] = result ? 1 : 0;
            Log.Log(li);
        }
    }
}