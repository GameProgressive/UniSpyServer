using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal sealed class ChatIRCNickNameInUseException : IRCException
    {
        private string _oldNickName;
        private string _newNickName;
        public override string ErrorResponse => IRCReplyBuilder.Build(IRCErrorCode.NickNameInUse, $"{_oldNickName} {_newNickName} *");
        public ChatIRCNickNameInUseException()
        {
        }

        public ChatIRCNickNameInUseException(string message, string oldNick, string newNick) : base(message, IRCErrorCode.NickNameInUse)
        {
            _oldNickName = oldNick;
            _newNickName = newNick;
        }

        public ChatIRCNickNameInUseException(string message, string oldNick, string newNick, System.Exception innerException) : base(message, IRCErrorCode.NickNameInUse, innerException)
        {
            _oldNickName = oldNick;
            _newNickName = newNick;
        }
    }
}