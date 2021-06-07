using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal class ChatIRCRegisterNickFaildException : ChatIRCException
    {
        public ChatIRCRegisterNickFaildException()
        {
        }

        public ChatIRCRegisterNickFaildException(string message) : base(message, ChatIRCErrorCode.RegisterNickFailed)
        {
        }

        public ChatIRCRegisterNickFaildException(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.RegisterNickFailed, innerException)
        {
        }
    }
}