using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCUniqueNickExpiredException : ChatIRCException
    {
        public ChatIRCUniqueNickExpiredException()
        {
        }

        public ChatIRCUniqueNickExpiredException(string message) : base(message, ChatIRCErrorCode.UniqueNIickExpired)
        {
        }

        public ChatIRCUniqueNickExpiredException(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.UniqueNIickExpired, innerException)
        {
        }
    }
}