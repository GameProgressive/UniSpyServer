using System;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyException : Exception
    {
        public UniSpyException()
        {
        }

        public UniSpyException(string message) : base(message)
        {
        }

        public UniSpyException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }

}
