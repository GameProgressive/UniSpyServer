using Chat.Entity.Structure;

namespace Chat.Abstraction.BaseClass
{
    public class ChatChannelRequestBase : ChatRequestBase
    {
        public string ChannelName { get; set; }
        public ChatChannelRequestBase(){}
        public ChatChannelRequestBase(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            if(ErrorCode != ChatErrorCode.NoError)
            {
                return;
            }

            if (_cmdParams.Count < 1)
            {
                ErrorCode = ChatErrorCode.Parse;
            }
            ChannelName = _cmdParams[0];
        }
    }
}
