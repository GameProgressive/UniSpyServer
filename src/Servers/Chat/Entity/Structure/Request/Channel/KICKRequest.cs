using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    internal sealed class KICKRequest : ChatChannelRequestBase
    {
        public KICKRequest() { }
        public KICKRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; set; }
        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            if (_cmdParams.Count != 2)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }
            NickName = _cmdParams[1];
            if (_longParam == null)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }
            Reason = _longParam;
        }
    }
}
