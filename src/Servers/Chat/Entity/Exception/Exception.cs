using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Exception
{
    internal sealed class Exception : UniSpyException
    {
        public Exception()
        {
        }

        public Exception(string message) : base(message)
        {
        }

        public Exception(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}