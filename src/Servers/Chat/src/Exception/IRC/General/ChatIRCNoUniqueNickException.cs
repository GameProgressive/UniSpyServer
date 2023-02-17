using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Exception.IRC.General
{
    public sealed class ChatIRCNoUniqueNickException : IRCException
    {
        public ChatIRCNoUniqueNickException(){ }

        public ChatIRCNoUniqueNickException(string message) : base(message, IRCErrorCode.NoUniqueNick){ }

        public ChatIRCNoUniqueNickException(string message, System.Exception innerException) : base(message, IRCErrorCode.NoUniqueNick, innerException){ }
    }
}