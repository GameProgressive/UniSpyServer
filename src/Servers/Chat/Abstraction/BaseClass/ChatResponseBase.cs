using Chat.Entity.Structure;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    public abstract class ChatResponseBase : UniSpyResponseBase
    {
        public new ChatErrorCode ErrorCode
        {
            get { return (ChatErrorCode)base.ErrorCode; }
            set { base.ErrorCode = value; }
        }
        public new string SendingBuffer
        {
            get { return (string)base.SendingBuffer; }
            set { base.SendingBuffer = value; }
        }

        public ChatResponseBase(ChatResultBase result) : base(result)
        {
            ErrorCode = ChatErrorCode.NoError;
        }

    }
}
