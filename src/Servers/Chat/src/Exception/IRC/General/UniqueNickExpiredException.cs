using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class UniqueNickExpiredException : IRCException
    {
        public UniqueNickExpiredException(){ }

        public UniqueNickExpiredException(string message) : base(message, IRCErrorCode.UniqueNIickExpired){ }

        public UniqueNickExpiredException(string message, System.Exception innerException) : base(message, IRCErrorCode.UniqueNIickExpired, innerException){ }
    }
}