using System;

namespace Ecis.Common.Exceptions
{
    /// <summary>
    /// 应用程序异常
    /// </summary>
    [Serializable]
    public class EcisApplicationException : EcisExceptionBase
    {
        protected static string titleMsg = Ecis.Common.Exceptions.Properties.Resources.AppTitleMsg;

        public EcisApplicationException()
        {
        }

        public EcisApplicationException(string message, ErrorMsgType emType = ErrorMsgType.Error)
            : base(titleMsg + message)
        {
            string msg = titleMsg + message;
            InfoMessage = LogExcept.LogByErrorMsgType(null, emType, msg);
        }

        public EcisApplicationException(string message, Exception inner, ErrorMsgType emType = ErrorMsgType.Error)
            : base(titleMsg + message, inner)
        {
            string msg = titleMsg + message;
            InfoMessage = LogExcept.LogByErrorMsgType(inner, emType, msg);
        }

        public override string ErrorCode
        {
            get { return ErrorCodeContract.ApplicationExceptionCode; }
        }

        public override string InfoMessage { get; set; }
    }
}