using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Exception.IRC.General
{
    public sealed class ChatIRCErrOneUSNickNameException : IRCException
    {
        public ChatIRCErrOneUSNickNameException(){ }

        public ChatIRCErrOneUSNickNameException(string message) : base(message, IRCErrorCode.ErrOneUSNickName){ }

        public ChatIRCErrOneUSNickNameException(string message, System.Exception innerException) : base(message, IRCErrorCode.ErrOneUSNickName, innerException){ }
    }
}