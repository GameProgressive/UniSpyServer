namespace Chat.Entity.Structure.ChatCommand.ChatChannel
{
    public class SETGROUP : ChatChannelCommandBase
    {
        public SETGROUP(string request) : base(request)
        {
        }
        public string GroupName { get; protected set; }
        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            if (_cmdParams.Count != 1)
            { return false; }
            GroupName = _cmdParams[0];
            return true;
        }
    }
}
