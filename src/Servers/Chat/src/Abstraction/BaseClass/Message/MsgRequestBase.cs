namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
{
    public enum MessageType
    {
        ChannelMessage,
        UserMessage
    }

    public class MsgRequestBase : ChannelRequestBase
    {
        public MsgRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public MessageType MessageType { get; protected set; }
        public string NickName { get; protected set; }
        public string Message { get; protected set; }

        public override void Parse()
        {
            base.Parse();

            if (ChannelName.Contains("#"))
            {
                MessageType = MessageType.ChannelMessage;
            }
            else
            {
                // todo check if there need user message
                MessageType = MessageType.UserMessage;
                ChannelName = null;
                NickName = _cmdParams[0];
            }
            Message = _longParam;
        }
    }
}
