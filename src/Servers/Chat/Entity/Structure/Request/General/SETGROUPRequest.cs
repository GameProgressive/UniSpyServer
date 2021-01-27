using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class SETGROUPRequest : ChatChannelRequestBase
    {
        public SETGROUPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string GroupName { get; protected set; }
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

            GroupName = _cmdParams[0];
        }
    }
}
