namespace Chat.Entity.Structure.ChatCommand
{
    public class SETGROUP : ChatChannelCommandBase
    {
        public string GroupName { get; protected set; }
        public override bool Parse(string request)
        {
            if (!base.Parse(request))
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
