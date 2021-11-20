using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("LOGINPREAUTH")]

    public sealed class LoginPreAuthRequest : RequestBase
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
