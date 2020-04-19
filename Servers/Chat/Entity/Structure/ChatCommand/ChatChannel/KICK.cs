namespace Chat.Entity.Structure.ChatCommand
{
    public class KICK : ChatChannelCommandBase
    {
        public string UserName { get; protected set; }
        public string Reason { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
                return false;
            if (_cmdParams.Count != 1)
            {
                return false;
            }
            UserName = _cmdParams[0];
            if (_longParam == null)
                return false;
            Reason = _longParam;
            return true;
        }
    }
}
