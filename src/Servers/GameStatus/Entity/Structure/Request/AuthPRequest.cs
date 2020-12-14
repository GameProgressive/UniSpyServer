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

        public override void Parse()
        {
           base.Parse();
            if (ErrorCode != GSErrorCode.NoError)
            {
                return;
            }
            if (RequestKeyValues.ContainsKey("pid") && RequestKeyValues.ContainsKey("resp"))
            {
                //we parse profileid here
                uint profileID;
                if (!uint.TryParse(RequestKeyValues["pid"], out profileID))
                {
                    ErrorCode = GSErrorCode.Parse;
                    return;
                }
                ProfileID = profileID;
                RequestType = AuthMethod.ProfileIDAuth;
            }
            else if (RequestKeyValues.ContainsKey("authtoken") && RequestKeyValues.ContainsKey("response"))
            {
                AuthToken = RequestKeyValues["authtoken"];
                Response = RequestKeyValues["response"];
                RequestType = AuthMethod.PartnerIDAuth;
            }
            else if (RequestKeyValues.ContainsKey("keyhash") && RequestKeyValues.ContainsKey("nick"))
            {
                RequestType = AuthMethod.CDkeyAuth;
                KeyHash = RequestKeyValues["keyhash"];
                Nick = RequestKeyValues["nick"];
            }
            else
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }

            ErrorCode = GSErrorCode.NoError;
        }
    }
}
