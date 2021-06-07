using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal class ChatIRCNickNameInUseException : ChatIRCException
    {
        public ChatIRCNickNameInUseException()
        {
        }

        public ChatIRCNickNameInUseException(string message) : base(message, ChatIRCErrorCode.NickNameInUse)
        {
        }

        public ChatIRCNickNameInUseException(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.NickNameInUse, innerException)
        {
        }
    }
}