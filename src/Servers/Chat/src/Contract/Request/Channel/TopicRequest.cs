using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Channel
{
    public enum TopicRequestType
    {
        GetChannelTopic,
        SetChannelTopic
    }

    
    public sealed class TopicRequest : ChannelRequestBase
    {
        public new string ChannelName{ get => base.ChannelName; set => base.ChannelName = value; }
        public TopicRequest(string rawRequest) : base(rawRequest){ }

        public string Channel { get; private set; }
        public string ChannelTopic { get; private set; }
        public TopicRequestType RequestType { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam is null)
            {
                RequestType = TopicRequestType.GetChannelTopic;
                return;
            }

            RequestType = TopicRequestType.SetChannelTopic;
            ChannelTopic = _longParam;
        }
    }
}
