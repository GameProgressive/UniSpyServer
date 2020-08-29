namespace Chat.Entity.Structure.ChatCommand
{
    public class INVITE : ChatRequestBase
    {
        public INVITE(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; protected set; }
        public string UserName { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }
            if (_cmdParams.Count != 2)
            { return false; }

            ChannelName = _cmdParams[0];
            UserName = _cmdParams[1];
            return true;
        }
    }
}
