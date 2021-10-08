using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCErrOneUSNickNameException : IRCException
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