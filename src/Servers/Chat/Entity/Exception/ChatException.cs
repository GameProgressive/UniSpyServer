using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Exception
{
    internal sealed class ChatException : UniSpyExceptionBase
    {
        public ChatException()
        {
        }

        public ChatException(string message) : base(message)
        {
        }

        public ChatException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}