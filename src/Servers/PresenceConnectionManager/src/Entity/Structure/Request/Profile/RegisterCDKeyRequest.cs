using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Request
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
