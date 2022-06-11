using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    

    public sealed class LoginPreAuth : RequestBase
    {
        public LoginPreAuth(string rawRequest) : base(rawRequest){ }

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
