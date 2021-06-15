namespace Chat.Abstraction.BaseClass
{
    public enum ChatMessageType
    {
        ChannelMessage,
        UserMessage
    }

    internal class ChatMsgRequestBase : ChatChannelRequestBase
    {
        public ChatMsgRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public ChatMessageType MessageType { get; protected set; }
        public string NickName { get; protected set; }
        public string Message { get; protected set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams[0].Contains("#"))
            {
                MessageType = ChatMessageType.ChannelMessage;
            }
            else
            {
                MessageType = ChatMessageType.UserMessage;
                ChannelName = null;
                NickName = _cmdParams[0];
            }
            Message = _longParam;
        }
    }
}
