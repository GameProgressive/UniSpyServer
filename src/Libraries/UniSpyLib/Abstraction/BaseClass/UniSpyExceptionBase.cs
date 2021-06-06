using System;
using System.Runtime.Serialization;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyExceptionBase : Exception
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
    }

}
