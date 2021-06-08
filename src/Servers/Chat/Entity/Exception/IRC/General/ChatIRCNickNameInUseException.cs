using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Exception.IRC.General
{
    internal class ChatIRCNickNameInUseException : ChatIRCException
    {
        private string _oldNickName;
        private string _newNickName;
        public override string ErrorResponse => ChatIRCReplyBuilder.Build(ChatIRCErrorCode.NickNameInUse, $"{_oldNickName} {_newNickName} *");
        public ChatIRCNickNameInUseException()
        {
        }

        public ChatIRCNickNameInUseException(string message, string oldNick, string newNick) : base(message, ChatIRCErrorCode.NickNameInUse)
        {
            _oldNickName = oldNick;
            _newNickName = newNick;
        }

        public ChatIRCNickNameInUseException(string message, string oldNick, string newNick, System.Exception innerException) : base(message, ChatIRCErrorCode.NickNameInUse, innerException)
        {
            _oldNickName = oldNick;
            _newNickName = newNick;
        }
    }
}