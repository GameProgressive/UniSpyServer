using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCNoUniqueNickException : IRCException
    {
        public ChatIRCNoUniqueNickException()
        {
        }

        public ChatIRCNoUniqueNickException(string message) : base(message, IRCErrorCode.NoUniqueNick)
        {
        }

        public ChatIRCNoUniqueNickException(string message, System.Exception innerException) : base(message, IRCErrorCode.NoUniqueNick, innerException)
        {
        }
    }
}