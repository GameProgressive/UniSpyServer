namespace Chat.Entity.Structure.ChatCommand
{
    public class TOPIC : ChatChannelCommandBase
    {
        public string Topic { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            if (_longParam == null)
            {
                Topic = "";
            }
            else
            {
                Topic = _longParam;
            }
            return true;
        }
    }
}
