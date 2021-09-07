using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("LOGINPREAUTH")]

    internal sealed class LoginPreAuthRequest : RequestBase
    {
        public LoginPreAuthRequest(string rawRequest) : base(rawRequest)
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
