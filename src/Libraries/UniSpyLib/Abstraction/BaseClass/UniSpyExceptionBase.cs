using System;
using System.Runtime.Serialization;

namespace UniSpyLib.Abstraction.BaseClass
{
    public class UniSpyExceptionBase : Exception
    {
        public UniSpyExceptionBase()
        {
        }

        public UniSpyExceptionBase(string message) : base(message)
        {
        }

        public UniSpyExceptionBase(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UniSpyExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
