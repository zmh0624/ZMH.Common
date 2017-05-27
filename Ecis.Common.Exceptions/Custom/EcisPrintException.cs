using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZMH.Common.Exceptions
{
    public class EcisPrintException : EcisException
    {
        public EcisPrintException()
        {
        }

        public EcisPrintException(Exception inner, ErrorMsgType emType = ErrorMsgType.Error)
            : base(Properties.Resources.EcisPrintMsg, inner, emType) 
        {
            
        }
    }
}
