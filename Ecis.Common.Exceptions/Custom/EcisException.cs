using System;

namespace Ecis.Common.Exceptions
{
    /// <summary>
    /// 常规异常
    /// </summary>
    [Serializable]
    public class EcisException : EcisExceptionBase
    {
        protected static string titleMsg = Ecis.Common.Exceptions.Properties.Resources.TitleMsg;

        public EcisException()
        {
        }

        public EcisException(string message, ErrorMsgType emType = ErrorMsgType.Error)
            : base(titleMsg + message)
        {
            string msg = titleMsg + message;
            InfoMessage = LogExcept.LogByErrorMsgType(null, emType, msg);
        }

        public EcisException(string message, Exception inner, ErrorMsgType emType = ErrorMsgType.Error)
            : base(titleMsg + message, inner)
        {
            string msg = titleMsg + message;
            InfoMessage = LogExcept.LogByErrorMsgType(inner, emType, msg);
        }

        /// <summary>
        /// Ecis自定义错误Code
        /// </summary>
        public override string ErrorCode
        {
            get { return ErrorCodeContract.ExceptionCode; }
        }

        /// <summary>
        /// 友好提示信息
        /// </summary>
        public override string InfoMessage { get; set; }
    }
}