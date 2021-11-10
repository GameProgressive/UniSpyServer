using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.General
{
    public sealed class ChatIRCErrOneUSNickNameException : IRCException
    {
        public ChatIRCErrOneUSNickNameException()
        {
        }

        public ChatIRCErrOneUSNickNameException(string message) : base(message, IRCErrorCode.ErrOneUSNickName)
        {
        }

        public ChatIRCErrOneUSNickNameException(string message, System.Exception innerException) : base(message, IRCErrorCode.ErrOneUSNickName, innerException)
        {
        }
    }
}