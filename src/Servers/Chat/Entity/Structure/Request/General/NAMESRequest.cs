using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class NAMESRequest : ChatRequestBase
    {
        public NAMESRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if(!ErrorCode)
            {
               ErrorCode = false;
                return;
            }

            if (_cmdParams.Count != 1)
            {
                ErrorCode = false;
                return;
            }
            ChannelName = _cmdParams[0];
            ErrorCode = true;
        }
    }
}
