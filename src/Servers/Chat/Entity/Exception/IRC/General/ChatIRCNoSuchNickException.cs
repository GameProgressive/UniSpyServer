using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCNoSuchNickException : IRCException
    {
        public ChatIRCNoSuchNickException()
        {
        }

        public ChatIRCNoSuchNickException(string message) : base(message, IRCErrorCode.NoSuchNick)
        {
        }

        public ChatIRCNoSuchNickException(string message, System.Exception innerException) : base(message, IRCErrorCode.NoSuchNick, innerException)
        {
        }
    }
}