using System;

namespace UniSpy.Server.Core.Abstraction.BaseClass
{
    public class UniSpyException : Exception
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
