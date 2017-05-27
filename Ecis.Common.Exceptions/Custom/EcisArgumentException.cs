using System;

namespace ZMH.Common.Exceptions
{
    /// <summary>
    /// 参数异常
    /// </summary>
    [Serializable]
    public class EcisArgumentException : EcisExceptionBase
    {
        protected static string titleMsg = Properties.Resources.ArgTitleMsg;

        public EcisArgumentException()
        {
        }

        public EcisArgumentException(string message, ErrorMsgType emType = ErrorMsgType.Error)
            : base(titleMsg + message)
        {
            string msg = titleMsg + message;
            InfoMessage = LogExcept.LogByErrorMsgType(null, emType, msg);
        }

        public EcisArgumentException(string message, Exception inner, ErrorMsgType emType = ErrorMsgType.Error)
            : base(titleMsg + message, inner)
        {
            string msg = titleMsg + message;
            InfoMessage = LogExcept.LogByErrorMsgType(inner, emType, msg);
        }

        public override string ErrorCode
        {
            get { return ErrorCodeContract.ArgumentExceptionCode; }
        }

        public override string InfoMessage { get; set; }
    }
}