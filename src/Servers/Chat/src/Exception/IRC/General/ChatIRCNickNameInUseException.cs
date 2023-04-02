using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;

namespace UniSpy.Server.Chat.Error.IRC.General
{
    public sealed class ChatIRCNickNameInUseException : IRCException
    {
        private string _oldNickName;
        private string _newNickName;
        public ChatIRCNickNameInUseException() { }

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
        public override void Build()
        {
            SendingBuffer = $":{ResponseBase.ServerDomain} {IRCErrorCode.NickNameInUse} {_oldNickName} {_newNickName} *\r\n";
        }
    }
}