using System;

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

        public UniSpyExceptionBase(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }

}
