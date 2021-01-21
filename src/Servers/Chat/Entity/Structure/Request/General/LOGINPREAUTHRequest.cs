using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class LOGINPREAUTHRequest : ChatRequestBase
    {
        public LOGINPREAUTHRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string AuthToken { get; protected set; }
        public string PartnerChallenge { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }

            AuthToken = _cmdParams[0];
            PartnerChallenge = _cmdParams[1];
            ErrorCode = true;
        }
    }
}
