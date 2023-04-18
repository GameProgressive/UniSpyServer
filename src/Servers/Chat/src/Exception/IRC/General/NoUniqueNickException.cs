using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class NoUniqueNickException : IRCException
    {
        public NoUniqueNickException(){ }

        public NoUniqueNickException(string message) : base(message, IRCErrorCode.NoUniqueNick){ }

        public NoUniqueNickException(string message, System.Exception innerException) : base(message, IRCErrorCode.NoUniqueNick, innerException){ }
    }
}