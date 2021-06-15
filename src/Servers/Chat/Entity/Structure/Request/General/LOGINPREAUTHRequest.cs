using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    internal sealed class LOGINPREAUTHRequest : ChatRequestBase
    {
        public LOGINPREAUTHRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string AuthToken { get; private set; }
        public string PartnerChallenge { get; private set; }

        public override void Parse()
        {
            base.Parse();


            AuthToken = _cmdParams[0];
            PartnerChallenge = _cmdParams[1];
        }
    }
}
