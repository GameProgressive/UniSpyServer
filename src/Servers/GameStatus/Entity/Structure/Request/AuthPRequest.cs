using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;

namespace GameStatus.Entity.Structure.Request
{

    internal sealed class AuthPRequest : GSRequestBase

    {
        public AuthMethod RequestType { get; private set; }
        public uint ProfileID { get; private set; }
        public string AuthToken { get; private set; }
        public string Response { get; private set; }
        public string KeyHash { get; private set; }
        public string Nick { get; private set; }

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
