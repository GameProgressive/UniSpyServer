using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Exception
{
    public sealed class Exception : UniSpyException
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