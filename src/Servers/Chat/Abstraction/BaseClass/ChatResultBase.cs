using Chat.Entity.Structure;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    public class ChatResultBase : UniSpyResultBase
    {
        public new ChatErrorCode ErrorCode
        {
            get { return (ChatErrorCode)base.ErrorCode; }
            protected set { base.ErrorCode = value; }
        }

        public ChatResultBase()
        {
            ErrorCode = ChatErrorCode.NoError;
        }
    }
}
