using Chat.Entity.Structure;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatResultBase : UniSpyResultBase
    {
        public new ChatErrorCode ErrorCode
        {
            get => (ChatErrorCode)base.ErrorCode;
            set => base.ErrorCode = value;
        }
        public string IRCErrorCode { get; set; }

        public ChatResultBase()
        {
            ErrorCode = ChatErrorCode.NoError;
        }
    }
}
