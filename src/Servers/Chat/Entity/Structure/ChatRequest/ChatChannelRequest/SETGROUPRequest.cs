namespace Chat.Entity.Structure.ChatCommand
{
    public class SETGROUPRequest : ChatChannelRequestBase
    {
        public SETGROUPRequest(string rawRequest) : base(rawRequest)
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
            {
                return false;
            }

            GroupName = _cmdParams[0];
            return true;
        }
    }
}
