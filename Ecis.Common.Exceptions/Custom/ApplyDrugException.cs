using System;

namespace ZMH.Common.Exceptions
{
    /// <summary>
    /// 开药异常
    /// </summary>
    public class ApplyDrugException : EcisException
    {
        public ApplyDrugException()
        {
        }

        public ApplyDrugException(Exception inner, ErrorMsgType emType = ErrorMsgType.Error)
            : base(Properties.Resources.ApplyDrugMsg, inner, emType)
        {
        }
    }
}