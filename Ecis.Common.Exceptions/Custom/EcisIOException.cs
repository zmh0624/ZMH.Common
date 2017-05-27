using System;

namespace Ecis.Common.Exceptions.Custom
{
    [Serializable]
    public class EcisIOException : EcisExceptionBase
    {
        protected static string titleMsg = Ecis.Common.Exceptions.Properties.Resources.IOTitleMsg;

        public EcisIOException()
        {
        }

        public EcisIOException(string message, ErrorMsgType emType = ErrorMsgType.Error)
            : base(titleMsg + message)
        {
            string msg = titleMsg + message;
            InfoMessage = LogExcept.LogByErrorMsgType(null, emType, msg);
        }

        public EcisIOException(string message, Exception inner, ErrorMsgType emType = ErrorMsgType.Error)
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