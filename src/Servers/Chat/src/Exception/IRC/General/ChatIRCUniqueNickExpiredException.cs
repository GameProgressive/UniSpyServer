using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Exception.IRC.General
{
    public sealed class ChatIRCUniqueNickExpiredException : IRCException
    {
        public ChatIRCUniqueNickExpiredException(){ }

        public ChatIRCUniqueNickExpiredException(string message) : base(message, IRCErrorCode.UniqueNIickExpired){ }

        public ChatIRCUniqueNickExpiredException(string message, System.Exception innerException) : base(message, IRCErrorCode.UniqueNIickExpired, innerException){ }
    }
}