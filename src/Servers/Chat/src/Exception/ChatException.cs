using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Exception
{
    public sealed class ChatException : UniSpyException
    {
        public ChatException(){ }

        public ChatException(string message) : base(message){ }

        public ChatException(string message, System.Exception innerException) : base(message, innerException){ }
    }
}