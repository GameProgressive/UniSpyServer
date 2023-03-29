using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Enumerate;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Contract.Request
{
    // worm3d \authp\\pid\1\resp\7b6658e99f448388fbeddc93654e6dd4\lid\295[19][17]R([1B]zm}BKy[16]+sOhT[07][7F]{/[04]sz;j[00][15]<r[12]:j<[02][1B]:rOeDilR@0yA6[12]i>RG=[16][1B]jBP9\final\
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

            if (KeyValues.ContainsKey("pid") && KeyValues.ContainsKey("resp"))
            {
                //we parse profileid here
                int profileID;
                if (!int.TryParse(KeyValues["pid"], out profileID))
                {
                    throw new GSException("pid format is incorrect.");
                }
                ProfileId = profileID;
                RequestType = AuthMethod.ProfileIdAuth;
            }
            else if (KeyValues.ContainsKey("authtoken") && KeyValues.ContainsKey("response"))
            {
                AuthToken = KeyValues["authtoken"];
                Response = KeyValues["response"];
                RequestType = AuthMethod.PartnerIdAuth;
            }
            else if (KeyValues.ContainsKey("keyhash") && KeyValues.ContainsKey("nick"))
            {
                RequestType = AuthMethod.CDkeyAuth;
                CdKeyHash = KeyValues["keyhash"];
                NickName = KeyValues["nick"];
            }
            else
            {
                throw new GSException("Unknown authp request method.");
            }
        }
    }
}
