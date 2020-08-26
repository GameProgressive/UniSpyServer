using System;
using System.Collections.Generic;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.Structure.Request.General
{
    /// <summary>
    /// \registercdkey\sesskey\<sesskey>\cdkeyenc\<cdkeyenc>\id\<id>\final\
    /// </summary>
    public class RegisterCDKeyRequest : PCMRequest
    {
        public string CDKeyEnc { get; protected set; }
        public RegisterCDKeyRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPError  Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("cdkeyenc"))
            {
                return GPError.Parse;
            }

            CDKeyEnc = _recv["cdkeyenc"];
            return GPError.NoError;
        }
    }
}
