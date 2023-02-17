using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Enumerate;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Contract.Request
{
    
    public sealed class AuthPlayerRequest : RequestBase
    {
        public AuthMethod RequestType { get; private set; }
        public int ProfileId { get; private set; }
        public string AuthToken { get; private set; }
        public string Response { get; private set; }
        public string CdKeyHash { get; private set; }
        public string NickName { get; private set; }

        public AuthPlayerRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (PlayerData.ContainsKey("pid") && PlayerData.ContainsKey("resp"))
            {
                //we parse profileid here
                int profileID;
                if (!int.TryParse(PlayerData["pid"], out profileID))
                {
                    throw new GSException("pid format is incorrect.");
                }
                ProfileId = profileID;
                RequestType = AuthMethod.ProfileIDAuth;
            }
            else if (PlayerData.ContainsKey("authtoken") && PlayerData.ContainsKey("response"))
            {
                AuthToken = PlayerData["authtoken"];
                Response = PlayerData["response"];
                RequestType = AuthMethod.PartnerIDAuth;
            }
            else if (PlayerData.ContainsKey("keyhash") && PlayerData.ContainsKey("nick"))
            {
                RequestType = AuthMethod.CDkeyAuth;
                CdKeyHash = PlayerData["keyhash"];
                NickName = PlayerData["nick"];
            }
            else
            {
                throw new GSException("Unknown authp request method.");
            }
        }
    }
}
