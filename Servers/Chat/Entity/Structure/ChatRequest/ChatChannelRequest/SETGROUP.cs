namespace Chat.Entity.Structure.ChatCommand
{
    public class SETGROUP : ChatChannelRequestBase
    {
        public SETGROUP(string rawRequest) : base(rawRequest)
        {
        }

        public string GroupName { get; protected set; }
        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }

            if (_cmdParams.Count != 1)
            {
                return false;
            }

            GroupName = _cmdParams[0];
            return true;
        }
    }
}
