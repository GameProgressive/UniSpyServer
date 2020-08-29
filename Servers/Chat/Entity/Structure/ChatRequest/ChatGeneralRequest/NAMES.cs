namespace Chat.Entity.Structure.ChatCommand
{
    public class NAMES : ChatRequestBase
    {
        public NAMES(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }
            if (_cmdParams.Count != 1)
                return false;
            ChannelName = _cmdParams[0];
            return true;
        }
    }
}
