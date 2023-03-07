namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public enum MessageType
    {
        ChannelMessage,
        UserMessage
    }

    public abstract class MessageRequestBase : ChannelRequestBase
    {
        public MessageRequestBase(string rawRequest) : base(rawRequest){ }
        public MessageType? Type { get; protected set; }
        public string NickName { get; protected set; }
        public string Message { get; protected set; }

        public override void Parse()
        {
            base.Parse();

            if (ChannelName.Contains("#"))
            {
                Type = MessageType.ChannelMessage;
            }
            else
            {
                // todo check if there need user message
                Type = MessageType.UserMessage;
                ChannelName = null;
                NickName = _cmdParams[0];
            }
            Message = _longParam;
        }
    }
}
