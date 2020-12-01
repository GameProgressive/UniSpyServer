using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class NAMESRequest : ChatRequestBase
    {
        public NAMESRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; protected set; }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }

            if (_cmdParams.Count != 1)
                return false;
            ChannelName = _cmdParams[0];
            return true;
        }
    }
}
