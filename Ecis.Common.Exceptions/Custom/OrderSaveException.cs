using System;

namespace Ecis.Common.Exceptions
{
    public class OrderSaveException : EcisException
    {
        public OrderSaveException()
        {
        }

        public OrderSaveException(Exception inner, ErrorMsgType emType = ErrorMsgType.Error)
            : base(Properties.Resources.OrderSaveMsg, inner, emType)
        {
        }
    }
}