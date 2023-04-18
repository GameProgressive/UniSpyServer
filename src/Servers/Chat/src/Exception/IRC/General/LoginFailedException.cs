using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class LoginFailedException : IRCException
    {
        public LoginFailedException(){ }

        public LoginFailedException(string message) : base(message, IRCErrorCode.LoginFailed){ }

        public LoginFailedException(string message, System.Exception innerException) : base(message, IRCErrorCode.LoginFailed, innerException){ }
    }
}