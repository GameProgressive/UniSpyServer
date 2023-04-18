using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class MoreParametersException : IRCException
    {
        public MoreParametersException(){ }

        public MoreParametersException(string message) : base(message, IRCErrorCode.MoreParameters){ }

        public MoreParametersException(string message, System.Exception innerException) : base(message, IRCErrorCode.MoreParameters, innerException){ }
    }
}