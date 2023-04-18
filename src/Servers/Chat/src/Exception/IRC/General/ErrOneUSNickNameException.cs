using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class ErrOneUSNickNameException : IRCException
    {
        public ErrOneUSNickNameException(){ }

        public ErrOneUSNickNameException(string message) : base(message, IRCErrorCode.ErrOneUSNickName){ }

        public ErrOneUSNickNameException(string message, System.Exception innerException) : base(message, IRCErrorCode.ErrOneUSNickName, innerException){ }
    }
}