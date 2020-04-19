namespace Chat.Entity.Structure.ChatCommand
{
    public class WHO : ChatCommandBase
    {
        //TODO becareful there are channel name
        public string ChannelName { get; protected set; }
        public string NickName { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
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
