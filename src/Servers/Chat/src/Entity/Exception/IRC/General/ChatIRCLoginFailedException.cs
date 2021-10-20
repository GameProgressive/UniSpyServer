using UniSpyServer.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Chat.Entity.Exception.IRC.General
{
    public sealed class ChatIRCLoginFailedException : IRCException
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