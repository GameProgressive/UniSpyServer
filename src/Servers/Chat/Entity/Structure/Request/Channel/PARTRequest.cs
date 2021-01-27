using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    internal sealed class PARTRequest : ChatChannelRequestBase
    {
        public PARTRequest() { }
        public PARTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != ChatErrorCode.NoError)
            {
                return;
            }

            Reason = _longParam;
        }
    }
}
