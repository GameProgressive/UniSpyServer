using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal class ChatIRCNoUniqueNickException : ChatIRCException
    {
        public ChatIRCNoUniqueNickException()
        {
        }

        public ChatIRCNoUniqueNickException(string message) : base(message, ChatIRCErrorCode.NoUniqueNick)
        {
        }

        public ChatIRCNoUniqueNickException(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.NoUniqueNick, innerException)
        {
        }
    }
}