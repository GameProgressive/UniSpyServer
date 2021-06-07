using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal class ChatIRCMoreParametersException : ChatIRCException
    {
        public ChatIRCMoreParametersException()
        {
        }

        public ChatIRCMoreParametersException(string message) : base(message, ChatIRCErrorCode.MoreParameters)
        {
        }

        public ChatIRCMoreParametersException(string message, System.Exception innerException) : base(message, ChatIRCErrorCode.MoreParameters, innerException)
        {
        }
    }
}