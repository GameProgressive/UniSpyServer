using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Profile
{
    [RequestContract("registernick")]
    public sealed class RegisterNickRequest : RequestBase
    {
        public string UniqueNick { get; private set; }
        public string SessionKey { get; private set; }
        public RegisterNickRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("sesskey"))
            {
                throw new GPParseException("sesskey is missing.");
            }

            if (!RequestKeyValues.ContainsKey("uniquenick"))
            {
                throw new GPParseException("uniquenick is missing.");
            }
            SessionKey = RequestKeyValues["sesskey"];
            UniqueNick = RequestKeyValues["uniquenick"];
        }
    }
}
