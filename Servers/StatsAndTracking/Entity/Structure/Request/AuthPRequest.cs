using System;
using System.Collections.Generic;
using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Entity.Structure.Request
{

    public class AuthPRequest : GStatsRequestBase

    {
        public AuthMethod RequestType { get; protected set; }
        public uint ProfileID { get; protected set; }
        public string AuthToken { get; protected set; }
        public string Response { get; protected set; }
        public string KeyHash { get; protected set; }
        public string Nick { get; protected set; }

        public AuthPRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.NoError)
            {
                return flag;
            }
            if (_recv.ContainsKey("pid") && _recv.ContainsKey("resp"))
            {
                //we parse profileid here
                uint profileID;
                if (!uint.TryParse(_recv["pid"], out profileID))
                {
                    return GStatsErrorCode.Parse;
                }
                ProfileID = profileID;
                RequestType = AuthMethod.ProfileIDAuth;
            }
            else if (_recv.ContainsKey("authtoken") && _recv.ContainsKey("response"))
            {
                AuthToken = _recv["authtoken"];
                Response = _recv["response"];
                RequestType = AuthMethod.PartnerIDAuth;
            }
            else if (_recv.ContainsKey("keyhash") && _recv.ContainsKey("nick"))
            {
                RequestType = AuthMethod.CDkeyAuth;
                KeyHash = _recv["keyhash"];
                Nick = _recv["nick"];
            }
            else
            {
                return GStatsErrorCode.Parse;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
