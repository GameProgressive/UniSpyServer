using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
{
    public enum TopicRequestType
    {
        GetChannelTopic,
        SetChannelTopic
    }

    [RequestContract("TOPIC")]
    public sealed class TopicRequest : ChannelRequestBase
    {
        public TopicRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelTopic { get; private set; }
        public TopicRequestType RequestType { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                RequestType = TopicRequestType.GetChannelTopic;
            }
            else
            {
                RequestType = TopicRequestType.SetChannelTopic;
                ChannelTopic = _longParam;
            }
        }
    }
}
