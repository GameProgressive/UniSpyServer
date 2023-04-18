using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class RegisterNickFaildException : IRCException
    {
        public RegisterNickFaildException(){ }

        public RegisterNickFaildException(string message) : base(message, IRCErrorCode.RegisterNickFailed){ }

        public RegisterNickFaildException(string message, System.Exception innerException) : base(message, IRCErrorCode.RegisterNickFailed, innerException){ }
    }
}