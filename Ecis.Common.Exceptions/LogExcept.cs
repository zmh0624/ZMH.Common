using NLog;
using System;

namespace Ecis.Common.Exceptions
{
    public class LogExcept
    {
        private static ILogger _log;

        public static ILogger Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetCurrentClassLogger();
                }
                return _log;
            }
        }

        public static string LogByErrorMsgType(Exception inner, ErrorMsgType emType, string message)
        {
            switch (emType)
            {
                case ErrorMsgType.Info:
                    if (inner == null)
                    {
                        Log.Info(message);
                    }
                    else
                    {
                        Log.Info(inner, message);
                    }
                    return message;

                case ErrorMsgType.Warn:

                    if (inner == null)
                    {
                        Log.Warn(message);
                    }
                    else
                    {
                        Log.Warn(inner, message);
                    }
                    return message;

                case ErrorMsgType.Trace:
                    if (inner == null)
                    {
                        Log.Trace(message);
                    }
                    else
                    {
                        Log.Trace(inner, message);
                    }
                    return message;

                case ErrorMsgType.Debug:
                    if (inner == null)
                    {
                        Log.Debug(message);
                    }
                    else
                    {
                        Log.Debug(inner, message);
                    }
                    return message;

                case ErrorMsgType.Error:
                    if (inner == null)
                    {
                        Log.Error(message);
                    }
                    else
                    {
                        Log.Error(inner, message);
                    }
                    return message;

                case ErrorMsgType.Fatal:
                    if (inner == null)
                    {
                        Log.Fatal(message);
                    }
                    else
                    {
                        Log.Fatal(inner, message);
                    }
                    return message;

                default:
                    return null;
            }
        }
    }
}