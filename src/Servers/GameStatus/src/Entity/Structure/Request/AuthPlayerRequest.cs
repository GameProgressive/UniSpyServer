using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Enumerate;
using UniSpyServer.Servers.GameStatus.Entity.Exception;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
{
    
    public sealed class AuthPlayerRequest : RequestBase
    {
        public AuthMethod RequestType { get; private set; }
        public int ProfileId { get; private set; }
        public string AuthToken { get; private set; }
        public string Response { get; private set; }
        public string KeyHash { get; private set; }
        public string Nick { get; private set; }

        public AuthPlayerRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (KeyValues.ContainsKey("pid") && KeyValues.ContainsKey("resp"))
            {
                //we parse profileid here
                int profileID;
                if (!int.TryParse(KeyValues["pid"], out profileID))
                {
                    throw new GSException("pid format is incorrect.");
                }
                ProfileId = profileID;
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
                throw new GSException("Unknown authp request method.");
            }
        }
    }
}
