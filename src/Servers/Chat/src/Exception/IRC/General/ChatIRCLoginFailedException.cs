using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class ChatIRCLoginFailedException : IRCException
    {
        public ChatIRCLoginFailedException(){ }

        public ChatIRCLoginFailedException(string message) : base(message, IRCErrorCode.LoginFailed){ }

        public ChatIRCLoginFailedException(string message, System.Exception innerException) : base(message, IRCErrorCode.LoginFailed, innerException){ }
    }
}