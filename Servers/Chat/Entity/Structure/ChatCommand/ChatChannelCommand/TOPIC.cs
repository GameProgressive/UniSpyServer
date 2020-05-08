namespace Chat.Entity.Structure.ChatCommand
{
    public enum TOPICCmdType
    {
        GetChannelTopic,
        SetChannelTopic
    }

    public class TOPIC : ChatChannelCommandBase
    {
        public string ChannelTopic { get; protected set; }
        public TOPICCmdType RequestType { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
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
