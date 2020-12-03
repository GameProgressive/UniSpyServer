using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{

    public class AuthPRequest : GSRequestBase

    {
        public AuthMethod RequestType { get; protected set; }
        public uint ProfileID { get; protected set; }
        public string AuthToken { get; protected set; }
        public string Response { get; protected set; }
        public string KeyHash { get; protected set; }
        public string Nick { get; protected set; }

        public AuthPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
           var flag = (GSError)base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }
            if (KeyValues.ContainsKey("pid") && KeyValues.ContainsKey("resp"))
            {
                //we parse profileid here
                uint profileID;
                if (!uint.TryParse(KeyValues["pid"], out profileID))
                {
                    return GSError.Parse;
                }
                ProfileID = profileID;
                RequestType = AuthMethod.ProfileIDAuth;
            }
            else if (KeyValues.ContainsKey("authtoken") && KeyValues.ContainsKey("response"))
            {
                AuthToken = KeyValues["authtoken"];
                Response = KeyValues["response"];
                RequestType = AuthMethod.PartnerIDAuth;
            }
            else if (KeyValues.ContainsKey("keyhash") && KeyValues.ContainsKey("nick"))
            {
                RequestType = AuthMethod.CDkeyAuth;
                KeyHash = KeyValues["keyhash"];
                Nick = KeyValues["nick"];
            }
            else
            {
                return GSError.Parse;
            }

            return GSError.NoError;
        }
    }
}
