using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCLoginFailedException : IRCException
    {
        public ChatIRCLoginFailedException()
        {
        }

        public ChatIRCLoginFailedException(string message) : base(message, IRCErrorCode.LoginFailed)
        {
        }

        public ChatIRCLoginFailedException(string message, System.Exception innerException) : base(message, IRCErrorCode.LoginFailed, innerException)
        {
        }
    }
}