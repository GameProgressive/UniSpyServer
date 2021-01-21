using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class PARTRequest : ChatChannelRequestBase
    {
        public PARTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; protected set; }


        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }

            Reason = _longParam;
            ErrorCode = true;
        }
    }
}
