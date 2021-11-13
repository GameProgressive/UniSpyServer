using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Profile
{
    /// <summary>
    /// \registercdkey\sesskey\<sesskey>\cdkeyenc\<cdkeyenc>\id\<id>\final\
    /// </summary>
    [RequestContract("registercdkey")]
    public sealed class RegisterCDKeyRequest : RequestBase
    {
        public string CDKeyEnc { get; private set; }
        public RegisterCDKeyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("cdkeyenc"))
            {
                throw new GPParseException("cdkeyenc is missing.");
            }

            CDKeyEnc = RequestKeyValues["cdkeyenc"];
        }
    }
}
