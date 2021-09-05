using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public enum TOPICCmdType
    {
        GetChannelTopic,
        SetChannelTopic
    }

    internal sealed class TOPICRequest : ChatChannelRequestBase
    {
        public TOPICRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelTopic { get; private set; }
        public TOPICCmdType RequestType { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                RequestType = TOPICCmdType.GetChannelTopic;
            }
            else
            {
                RequestType = TOPICCmdType.SetChannelTopic;
                ChannelTopic = _longParam;
            }
        }
    }
}
