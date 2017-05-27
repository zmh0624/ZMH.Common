using ZMH.Common.Exceptions.Custom;
using System;
using System.Linq.Expressions;

namespace ZMH.Common.Exceptions
{
    public class ExceptUtil
    {
        /// <summary>
        /// 记录日志并抛出错误
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        /// <param name="emType"></param>
        public static void Throw(
            string errorCode,
            string message = null,
            Exception inner = null,
            ErrorMsgType emType = ErrorMsgType.Error)
        {
            switch (errorCode)
            {
                case ErrorCodeContract.OrderSaveExCode:
                    throw new OrderSaveException(inner, emType);

                case ErrorCodeContract.ApplyDrugExceptionCode:
                    throw new ApplyDrugException(inner, emType);

                case ErrorCodeContract.ExceptionCode:
                    throw new EcisException(message, inner, emType);

                case ErrorCodeContract.ApplicationExceptionCode:
                    throw new EcisApplicationException(message, inner, emType);

                case ErrorCodeContract.ArgumentExceptionCode:
                    throw new EcisArgumentException(message, inner, emType);

                case ErrorCodeContract.IOExceptionCode:
                    throw new EcisIOException(message, inner, emType);

                case ErrorCodeContract.PrintExceptionCode:
                    throw new EcisPrintException(inner, emType);
            }
        }

        /// <summary>
        /// 记录日志不抛出错误
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        /// <param name="emType"></param>
        public static void LogNoThrow(
            string errorCode,
            string message = null,
            Exception inner = null,
            ErrorMsgType emType = ErrorMsgType.Error)
        {
            switch (errorCode)
            {
                case ErrorCodeContract.OrderSaveExCode:
                    new OrderSaveException(inner, emType);
                    break;

                case ErrorCodeContract.ApplyDrugExceptionCode:
                    new ApplyDrugException(inner, emType);
                    break;

                case ErrorCodeContract.ExceptionCode:
                    new EcisException(message, inner, emType);
                    break;

                case ErrorCodeContract.ApplicationExceptionCode:
                    new EcisApplicationException(message, inner, emType);
                    break;

                case ErrorCodeContract.ArgumentExceptionCode:
                    new EcisArgumentException(message, inner, emType);
                    break;

                case ErrorCodeContract.IOExceptionCode:
                    new EcisIOException(message, inner, emType);
                    break;
                    
                case ErrorCodeContract.PrintExceptionCode:
                    new EcisPrintException(inner, emType);
                    break;
            }
        }

        public static void ThrowIfNull(Expression<Func<object>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body == null)
            {
                throw new EcisArgumentException("expected property or field expression.");
            }
            var compiled = expression.Compile();
            var value = compiled();
            if (value == null)
            {
                throw new EcisArgumentException(body.Member.Name);
            }
        }

        public static void ThrowIfNullOrEmpty(Expression<Func<String>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body == null)
            {
                throw new EcisArgumentException("expected property or field expression.");
            }
            var compiled = expression.Compile();
            var value = compiled();
            if (String.IsNullOrEmpty(value))
            {
                throw new EcisArgumentException(string.Format("String [{0}] is null or empty", body.Member.Name));
            }
        }
    }
}