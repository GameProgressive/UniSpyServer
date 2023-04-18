using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class NoSuchNickException : IRCException
    {
        public NoSuchNickException(){ }

        public NoSuchNickException(string message) : base(message, IRCErrorCode.NoSuchNick){ }

        public NoSuchNickException(string message, System.Exception innerException) : base(message, IRCErrorCode.NoSuchNick, innerException){ }
    }
}