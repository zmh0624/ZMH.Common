using System;

namespace ZMH.Common.Exceptions
{
    /// <summary>
    /// Ecis自定义异常基类
    /// </summary>
    public abstract class EcisExceptionBase : Exception
    {
        public EcisExceptionBase()
        {
        }

        public EcisExceptionBase(string message)
            : base(message)
        {
            LogExcept.Log.Error(message);
        }

        public EcisExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
            LogExcept.Log.Error(inner);
        }

        /// <summary>
        /// Ecis自定义错误Code
        /// </summary>
        public abstract string ErrorCode { get; }

        /// <summary>
        /// 异常友好信息
        /// </summary>
        public abstract string InfoMessage { get; set; }
    }
}