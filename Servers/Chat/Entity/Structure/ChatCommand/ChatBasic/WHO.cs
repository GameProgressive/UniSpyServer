namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class WHO : ChatCommandBase
    {
        //TODO becareful there are channel name
        public string ChannelName { get; protected set; }
        public string NickName { get; protected set; }
        public WHO(string request) : base(request)
        {
        }
        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            if (_cmdParams.Count != 1)
            { return false; }
            NickName = _cmdParams[0];
            return true;
        }
    }
}
