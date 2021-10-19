using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    public sealed class ChatIRCMoreParametersException : IRCException
    {
        public ChatIRCMoreParametersException()
        {
        }

        public ChatIRCMoreParametersException(string message) : base(message, IRCErrorCode.MoreParameters)
        {
        }

        public ChatIRCMoreParametersException(string message, System.Exception innerException) : base(message, IRCErrorCode.MoreParameters, innerException)
        {
        }
    }
}