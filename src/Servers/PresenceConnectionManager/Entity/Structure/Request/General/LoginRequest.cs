using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using Serilog.Events;
using UniSpyLib.Logging;
namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal class LoginRequest : PCMRequestBase
    {
        public string UserChallenge { get; protected set; }
        public string Response { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string UserData { get; protected set; }
        public uint NamespaceID { get; private set; }
        public string AuthToken { get; protected set; }
        public string Nick { get; protected set; }
        public string Email { get; protected set; }
        public uint ProductID { get; protected set; }
        public LoginType LoginType { get; protected set; }
        public SDKRevisionType SDKRevisionType { get; protected set; }

        public LoginRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            if (!KeyValues.ContainsKey("challenge"))
                throw new GPGeneralException("challenge is missing", GPErrorCode.Parse);

            if (!KeyValues.ContainsKey("response"))
            {
                throw new GPGeneralException("response is missing", GPErrorCode.Parse);
            }

            UserChallenge = KeyValues["challenge"];
            Response = KeyValues["response"];

            if (KeyValues.ContainsKey("uniquenick") && KeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(KeyValues["namespaceid"], out namespaceID))
                {
                    throw new GPGeneralException("namespaceid format is incorrect", GPErrorCode.Parse);
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
                    throw new GPGeneralException("user format is incorrect", GPErrorCode.Parse);
                }
                Nick = UserData.Substring(0, Pos);
                Email = UserData.Substring(Pos + 1);

                // we need to get namespaceid for email login
                if (KeyValues.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(KeyValues["namespaceid"], out namespaceID))
                    {
                        throw new GPGeneralException("namespaceid format is incorrect", GPErrorCode.Parse);
                    }
                    NamespaceID = namespaceID;
                }
            }
            else
            {
                throw new GPGeneralException("Unknown login method detected.", GPErrorCode.Parse);
            }

            ParseOtherData();
        }

        public int? GamePort { get; protected set; }
        public uint PartnerID { get; protected set; }
        public string GameName { get; private set; }
        public QuietModeType QuietModeFlags { get; private set; }

        private void ParseOtherData()
        {
            if (KeyValues.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(KeyValues["partnerid"], out partnerID))
                {
                    throw new GPGeneralException("partnerid format is incorrect", GPErrorCode.Parse);
                }
                PartnerID = partnerID;
            }

            //store sdkrevision
            if (KeyValues.ContainsKey("sdkrevision"))
            {
                uint sdkRevisionType;
                if (!uint.TryParse(KeyValues["sdkrevision"], out sdkRevisionType))
                {
                    throw new GPGeneralException("sdkrevision format is incorrect", GPErrorCode.Parse);
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
                    throw new GPGeneralException("port format is incorrect", GPErrorCode.Parse);
                }
                GamePort = htonGamePort;
            }
            if (KeyValues.ContainsKey("productid"))
            {
                uint productID;
                if (!uint.TryParse(KeyValues["productid"], out productID))
                {
                    throw new GPGeneralException("productid format is incorrect", GPErrorCode.Parse);
                }
                ProductID = productID;
            }
            if (KeyValues.ContainsKey("quiet"))
            {
                uint quiet;
                if (!uint.TryParse(KeyValues["quiet"], out quiet))
                {
                    throw new GPGeneralException("quiet format is incorrect", GPErrorCode.Parse);
                }

                QuietModeFlags = (QuietModeType)quiet;
            }
        }
    }
}
