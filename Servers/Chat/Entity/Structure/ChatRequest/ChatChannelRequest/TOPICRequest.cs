namespace Chat.Entity.Structure.ChatCommand
{
    public enum TOPICCmdType
    {
        GetChannelTopic,
        SetChannelTopic
    }

    public class TOPICRequest : ChatChannelRequestBase
    {
        public TOPICRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelTopic { get; protected set; }
        public TOPICCmdType RequestType { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }

            if (_longParam == null)
            {
                RequestType = TOPICCmdType.GetChannelTopic;
            }
            else
            {
                RequestType = TOPICCmdType.SetChannelTopic;
                ChannelTopic = _longParam;
            }
            return true;
        }
    }
}
