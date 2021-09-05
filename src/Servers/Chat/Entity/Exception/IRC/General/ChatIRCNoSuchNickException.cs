using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCNoSuchNickException : ChatIRCException
    {
        public ChatIRCNoSuchNickException()
        {
        }

        public ChatIRCNoSuchNickException(string message) : base(message, ChatIRCErrorCode.NoSuchNick)
        {
        }

        public ChatIRCNoSuchNickException(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.NoSuchNick, innerException)
        {
        }
    }
}