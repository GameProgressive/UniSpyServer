using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Enumerate;
using UniSpyServer.Servers.GameStatus.Entity.Exception;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
{
    [RequestContract("authp")]
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

            if (RequestKeyValues.ContainsKey("pid") && RequestKeyValues.ContainsKey("resp"))
            {
                //we parse profileid here
                int profileID;
                if (!int.TryParse(RequestKeyValues["pid"], out profileID))
                {
                    throw new GSException("pid format is incorrect.");
                }
                ProfileId = profileID;
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
                throw new GSException("Unknown authp request method.");
            }
        }
    }
}
