using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Exception
{
    public sealed class ChatException : UniSpyException
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