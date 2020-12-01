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

        public AuthPRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override GSError Parse()
        {
            var flag = base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }
            if (_rawRequest.ContainsKey("pid") && _rawRequest.ContainsKey("resp"))
            {
                //we parse profileid here
                uint profileID;
                if (!uint.TryParse(_rawRequest["pid"], out profileID))
                {
                    return GSError.Parse;
                }
                ProfileID = profileID;
                RequestType = AuthMethod.ProfileIDAuth;
            }
            else if (_rawRequest.ContainsKey("authtoken") && _rawRequest.ContainsKey("response"))
            {
                AuthToken = _rawRequest["authtoken"];
                Response = _rawRequest["response"];
                RequestType = AuthMethod.PartnerIDAuth;
            }
            else if (_rawRequest.ContainsKey("keyhash") && _rawRequest.ContainsKey("nick"))
            {
                RequestType = AuthMethod.CDkeyAuth;
                KeyHash = _rawRequest["keyhash"];
                Nick = _rawRequest["nick"];
            }
            else
            {
                return GSError.Parse;
            }

            return GSError.NoError;
        }
    }
}
