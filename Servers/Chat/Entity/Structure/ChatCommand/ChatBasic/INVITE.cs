namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class INVITE : ChatCommandBase
    {
        public string ChannelName { get; protected set; }
        public string UserName { get; protected set; }
        public INVITE(string request) : base(request)
        {
        }
        public override bool Parse()
        {
            if (!base.Parse())
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
