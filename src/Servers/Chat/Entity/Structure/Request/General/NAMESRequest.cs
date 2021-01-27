using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
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
            if (ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            if (_cmdParams.Count != 1)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }
            ChannelName = _cmdParams[0];
        }
    }
}
