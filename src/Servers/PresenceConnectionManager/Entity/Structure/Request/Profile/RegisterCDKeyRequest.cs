using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Contract;
using PresenceSearchPlayer.Entity.Exception.General;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    /// <summary>
    /// \registercdkey\sesskey\<sesskey>\cdkeyenc\<cdkeyenc>\id\<id>\final\
    /// </summary>
    [RequestContract("registercdkey")]
    internal sealed class RegisterCDKeyRequest : RequestBase
    {
        public string CDKeyEnc { get; private set; }
        public RegisterCDKeyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!KeyValues.ContainsKey("cdkeyenc"))
            {
                throw new GPParseException("cdkeyenc is missing.");
            }

            CDKeyEnc = KeyValues["cdkeyenc"];
        }
    }
}
