using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace  PresenceConnectionManager.Entity.Structure.Request
{
    /// <summary>
    /// \registercdkey\sesskey\<sesskey>\cdkeyenc\<cdkeyenc>\id\<id>\final\
    /// </summary>
    public class RegisterCDKeyRequest : PCMRequestBase
    {
        public string CDKeyEnc { get; protected set; }
        public RegisterCDKeyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if( ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!KeyValues.ContainsKey("cdkeyenc"))
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }

            CDKeyEnc = KeyValues["cdkeyenc"];
            ErrorCode = GPErrorCode.NoError;
        }
    }
}
