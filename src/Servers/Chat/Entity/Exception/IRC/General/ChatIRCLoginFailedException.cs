using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCLoginFailedException : ChatIRCException
    {
        public ChatIRCLoginFailedException()
        {
        }

        public ChatIRCLoginFailedException(string message) : base(message, ChatIRCErrorCode.LoginFailed)
        {
        }

        public ChatIRCLoginFailedException(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.LoginFailed, innerException)
        {
        }
    }
}