using System;
using System.Runtime.Serialization;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyExceptionBase : Exception
    {
        public object ErrorCode { get; private set; }
        public abstract string ErrorResponse { get; }
        public UniSpyExceptionBase()
        {
        }

        public UniSpyExceptionBase(string message, object errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public UniSpyExceptionBase(string message, object errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }

}
