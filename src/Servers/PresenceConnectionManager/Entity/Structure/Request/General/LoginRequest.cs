using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    [Command("login")]
    internal sealed class LoginRequest : PCMRequestBase
    {
        public string UserChallenge { get; private set; }
        public string Response { get; private set; }
        public string UniqueNick { get; private set; }
        public string UserData { get; private set; }
        public uint NamespaceID { get; private set; }
        public string AuthToken { get; private set; }
        public string Nick { get; private set; }
        public string Email { get; private set; }
        public uint ProductID { get; private set; }
        public LoginType LoginType { get; private set; }
        public SDKRevisionType SDKRevisionType { get; private set; }

        public LoginRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!KeyValues.ContainsKey("challenge"))
                throw new GPParseException("challenge is missing");

            if (!KeyValues.ContainsKey("response"))
            {
                throw new GPParseException("response is missing");
            }

            UserChallenge = KeyValues["challenge"];
            Response = KeyValues["response"];

            if (KeyValues.ContainsKey("uniquenick") && KeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(KeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPParseException("namespaceid format is incorrect");
                }
                LoginType = LoginType.UniquenickNamespaceID;
                UniqueNick = KeyValues["uniquenick"];
                UserData = UniqueNick;
                NamespaceID = namespaceID;
            }
            else if (KeyValues.ContainsKey("authtoken"))
            {
                LoginType = LoginType.AuthToken;
                AuthToken = KeyValues["authtoken"];
                UserData = AuthToken;
            }
            else if (KeyValues.ContainsKey("user"))
            {
                LoginType = LoginType.NickEmail;
                UserData = KeyValues["user"];

                int Pos = UserData.IndexOf('@');
                if (Pos == -1 || Pos < 1 || (Pos + 1) >= UserData.Length)
                {
                    throw new GPParseException("user format is incorrect");
                }
                Nick = UserData.Substring(0, Pos);
                Email = UserData.Substring(Pos + 1);

                // we need to get namespaceid for email login
                if (KeyValues.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(KeyValues["namespaceid"], out namespaceID))
                    {
                        throw new GPParseException("namespaceid format is incorrect");
                    }
                    NamespaceID = namespaceID;
                }
            }
            else
            {
                throw new GPParseException("Unknown login method detected.");
            }

            ParseOtherData();
        }

        public int? GamePort { get; private set; }
        public uint PartnerID { get; private set; }
        public string GameName { get; private set; }
        public QuietModeType QuietModeFlags { get; private set; }

        private void ParseOtherData()
        {
            if (KeyValues.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(KeyValues["partnerid"], out partnerID))
                {
                    throw new GPParseException("partnerid format is incorrect");
                }
                PartnerID = partnerID;
            }

            //store sdkrevision
            if (KeyValues.ContainsKey("sdkrevision"))
            {
                uint sdkRevisionType;
                if (!uint.TryParse(KeyValues["sdkrevision"], out sdkRevisionType))
                {
                    throw new GPParseException("sdkrevision format is incorrect");
                }

                SDKRevisionType = (SDKRevisionType)sdkRevisionType;
            }

            if (KeyValues.ContainsKey("gamename"))
            {
                GameName = KeyValues["gamename"];
            }

            if (KeyValues.ContainsKey("port"))
            {
                int htonGamePort;
                if (!int.TryParse(KeyValues["port"], out htonGamePort))
                {
                    throw new GPParseException("port format is incorrect");
                }
                GamePort = htonGamePort;
            }
            if (KeyValues.ContainsKey("productid"))
            {
                uint productID;
                if (!uint.TryParse(KeyValues["productid"], out productID))
                {
                    throw new GPParseException("productid format is incorrect");
                }
                ProductID = productID;
            }
            if (KeyValues.ContainsKey("quiet"))
            {
                uint quiet;
                if (!uint.TryParse(KeyValues["quiet"], out quiet))
                {
                    throw new GPParseException("quiet format is incorrect");
                }

                QuietModeFlags = (QuietModeType)quiet;
            }
        }
    }
}
