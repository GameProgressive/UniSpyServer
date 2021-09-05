using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCErrOneUSNickNameException : ChatIRCException
    {
        public ChatIRCErrOneUSNickNameException()
        {
        }

        public ChatIRCErrOneUSNickNameException(string message) : base(message, ChatIRCErrorCode.ErrOneUSNickName)
        {
        }

        public ChatIRCErrOneUSNickNameException(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.ErrOneUSNickName, innerException)
        {
        }
    }
}