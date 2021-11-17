using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.General
{
    [RequestContract("login")]
    public sealed class LoginRequest : RequestBase
    {
        public string UserChallenge { get; private set; }
        public string Response { get; private set; }
        public string UniqueNick { get; private set; }
        public string UserData { get; private set; }
        public uint? NamespaceID { get; private set; }
        public string AuthToken { get; private set; }
        public string Nick { get; private set; }
        public string Email { get; private set; }
        public uint? ProductID { get; private set; }
        public LoginType? Type { get; private set; }
        public SDKRevisionType? SDKRevisionType { get; private set; }

        public LoginRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!RequestKeyValues.ContainsKey("challenge"))
                throw new GPParseException("challenge is missing");

            if (!RequestKeyValues.ContainsKey("response"))
            {
                throw new GPParseException("response is missing");
            }

            UserChallenge = RequestKeyValues["challenge"];
            Response = RequestKeyValues["response"];

            if (RequestKeyValues.ContainsKey("uniquenick") && RequestKeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPParseException("namespaceid format is incorrect");
                }
                Type = LoginType.UniquenickNamespaceID;
                UniqueNick = RequestKeyValues["uniquenick"];
                UserData = UniqueNick;
                NamespaceID = namespaceID;
            }
            else if (RequestKeyValues.ContainsKey("authtoken"))
            {
                Type = LoginType.AuthToken;
                AuthToken = RequestKeyValues["authtoken"];
                UserData = AuthToken;
            }
            else if (RequestKeyValues.ContainsKey("user"))
            {
                Type = LoginType.NickEmail;
                UserData = RequestKeyValues["user"];

                int Pos = UserData.IndexOf('@');
                if (Pos == -1 || Pos < 1 || (Pos + 1) >= UserData.Length)
                {
                    throw new GPParseException("user format is incorrect");
                }
                Nick = UserData.Substring(0, Pos);
                Email = UserData.Substring(Pos + 1);

                // we need to get namespaceid for email login
                if (RequestKeyValues.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(RequestKeyValues["namespaceid"], out namespaceID))
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
        public uint? UserID { get; private set; }
        public uint? ProfileID { get; private set; }
        public uint? PartnerID { get; private set; }
        public string GameName { get; private set; }
        public QuietModeType? QuietModeFlags { get; private set; }
        public string Firewall { get; private set; }

        private void ParseOtherData()
        {
            if (RequestKeyValues.ContainsKey("userid"))
            {
                uint userID;
                if (!uint.TryParse(RequestKeyValues["userid"], out userID))
                {
                    throw new GPParseException("partnerid format is incorrect");
                }
                UserID = userID;

            }
            if (RequestKeyValues.ContainsKey("profileid"))
            {
                uint profileID;
                if (!uint.TryParse(RequestKeyValues["profileid"], out profileID))
                {
                    throw new GPParseException("profileid format is incorrect");
                }
                ProfileID = profileID;
            }
            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(RequestKeyValues["partnerid"], out partnerID))
                {
                    throw new GPParseException("partnerid format is incorrect");
                }
                PartnerID = partnerID;
            }

            //store sdkrevision
            if (RequestKeyValues.ContainsKey("sdkrevision"))
            {
                uint sdkRevisionType;
                if (!uint.TryParse(RequestKeyValues["sdkrevision"], out sdkRevisionType))
                {
                    throw new GPParseException("sdkrevision format is incorrect");
                }

                SDKRevisionType = (SDKRevisionType)sdkRevisionType;
            }

            if (RequestKeyValues.ContainsKey("gamename"))
            {
                GameName = RequestKeyValues["gamename"];
            }

            if (RequestKeyValues.ContainsKey("port"))
            {
                int htonGamePort;
                if (!int.TryParse(RequestKeyValues["port"], out htonGamePort))
                {
                    throw new GPParseException("port format is incorrect");
                }
                GamePort = htonGamePort;
            }
            if (RequestKeyValues.ContainsKey("productid"))
            {
                uint productID;
                if (!uint.TryParse(RequestKeyValues["productid"], out productID))
                {
                    throw new GPParseException("productid format is incorrect");
                }
                ProductID = productID;
            }

            if (RequestKeyValues.ContainsKey("firewall"))
            {
                Firewall = RequestKeyValues["firewall"];
            }

            if (RequestKeyValues.ContainsKey("quiet"))
            {
                uint quiet;
                if (!uint.TryParse(RequestKeyValues["quiet"], out quiet))
                {
                    throw new GPParseException("quiet format is incorrect");
                }

                QuietModeFlags = (QuietModeType)quiet;
            }
        }
    }
}
