using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class SETGROUPRequest : ChatChannelRequestBase
    {
        public SETGROUPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string GroupName { get; protected set; }
        public override object Parse()
        {
            if(!(bool)base.Parse())
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
