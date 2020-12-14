using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class INVITERequest : ChatRequestBase
    {
        public INVITERequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; protected set; }
        public string UserName { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if(!ErrorCode)
            {
               ErrorCode = false;
                return;
            }

            if (_cmdParams.Count != 2)
            {
                ErrorCode = false;
                return;
            }

            ChannelName = _cmdParams[0];
            UserName = _cmdParams[1];
            ErrorCode = true;
        }
    }
}
