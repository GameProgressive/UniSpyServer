using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.General
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

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!KeyValues.ContainsKey("cdkeyenc"))
            {
                return GPErrorCode.Parse;
            }

            CDKeyEnc = KeyValues["cdkeyenc"];
            return GPErrorCode.NoError;
        }
    }
}
