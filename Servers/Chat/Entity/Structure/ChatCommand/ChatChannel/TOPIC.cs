namespace Chat.Entity.Structure.ChatCommand
{
    public class TOPIC : ChatChannelCommandBase
    {
        public string ChannelTopic { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            if (_longParam == null)
            {
                ChannelTopic = "";
            }
            else
            {
                ChannelTopic = _longParam;
            }
            return true;
        }
    }
}
