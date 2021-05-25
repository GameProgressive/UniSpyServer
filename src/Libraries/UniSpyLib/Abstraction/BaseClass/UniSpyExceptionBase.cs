using System;
using System.Runtime.Serialization;

namespace UniSpyLib.Abstraction.BaseClass
{
    public class UniSpyExceptionBase : Exception
    {
        public object ErrorCode { get; protected set; }
        public UniSpyExceptionBase()
        {
        }

        public UniSpyExceptionBase(string message) : base(message)
        {
        }

        public UniSpyExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
