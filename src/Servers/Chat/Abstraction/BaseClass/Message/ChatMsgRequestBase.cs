using Chat.Entity.Structure;

namespace Chat.Abstraction.BaseClass
{
    public enum ChatMessageType
    {
        ChannelMessage,
        UserMessage
    }

    public class ChatMsgRequestBase : ChatChannelRequestBase
    {
        public ChatMsgRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public ChatMessageType RequestType { get; protected set; }
        public string NickName { get; protected set; }
        public string Message { get; protected set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams[0].Contains("#"))
            {
                RequestType = ChatMessageType.ChannelMessage;
            }
            else
            {
                RequestType = ChatMessageType.UserMessage;
                ChannelName = null;
                NickName = _cmdParams[0];
            }

            Message = _longParam;
        }
    }
}
