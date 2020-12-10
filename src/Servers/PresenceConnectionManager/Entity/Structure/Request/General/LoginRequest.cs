using UniSpyLib.Logging;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Enumerate;
using Serilog.Events;
using System.Collections.Generic;

namespace  PresenceConnectionManager.Entity.Structure.Request
{
    public class LoginRequest : PCMRequestBase
    {
        public string UserChallenge { get; protected set; }
        public string Response { get; protected set; }
        public LoginType LoginType { get; protected set; }
        public string Uniquenick { get; protected set; }
        public string UserData { get; protected set; }
        public uint NamespaceID { get; private set; }
        public string AuthToken { get; protected set; }
        public string Nick { get; protected set; }
        public string Email { get; protected set; }
        public SDKRevisionType SDKType { get; protected set; }

        public LoginRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!KeyValues.ContainsKey("challenge") || !KeyValues.ContainsKey("response"))
            {
                return GPErrorCode.Parse;
            }

            UserChallenge = KeyValues["challenge"];
            Response = KeyValues["response"];

            if (KeyValues.ContainsKey("uniquenick") && KeyValues.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(KeyValues["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }
                LoginType = LoginType.UniquenickNamespaceID;
                Uniquenick = KeyValues["uniquenick"];
                UserData = Uniquenick;
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
                    return GPErrorCode.Parse;
                }
                Nick = UserData.Substring(0, Pos);
                Email = UserData.Substring(Pos + 1);

                // we need to get namespaceid for email login
                if (KeyValues.ContainsKey("namespaceid"))
                {
                    uint namespaceID;
                    if (!uint.TryParse(KeyValues["namespaceid"], out namespaceID))
                    {
                        return GPErrorCode.Parse;
                    }
                    NamespaceID = namespaceID;
                }
            }
            else
            {
                LogWriter.ToLog(LogEventLevel.Error, "Unknown login method detected!");
                return GPErrorCode.Parse;
            }

            return ParseOtherData();
        }

        public uint GamePort { get; protected set; }
        public uint PartnerID { get; protected set; }
        public string GameName { get; private set; }
        public QuietModeType QuietMode { get; private set; }

        private GPErrorCode ParseOtherData()
        {
            if (KeyValues.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(KeyValues["partnerid"], out partnerID))
                {
                    return GPErrorCode.Parse;
                }
                PartnerID = partnerID;
            }

            //store sdkrevision
            if (KeyValues.ContainsKey("sdkrevision"))
            {
                uint sdkRevisionType;
                if (!uint.TryParse(KeyValues["sdkrevision"], out sdkRevisionType))
                {
                    return GPErrorCode.Parse;
                }

                SDKType = (SDKRevisionType)sdkRevisionType;
            }

            if (KeyValues.ContainsKey("gamename"))
            {
                GameName = KeyValues["gamename"];
            }

            if (KeyValues.ContainsKey("port"))
            {
                uint gamePort;
                if (!uint.TryParse(KeyValues["port"], out gamePort))
                {
                    return GPErrorCode.Parse;
                }
                GamePort = gamePort;
            }

            if (KeyValues.ContainsKey("quiet"))
            {
                uint quiet;
                if (!uint.TryParse(KeyValues["quiet"], out quiet))
                {
                    return GPErrorCode.Parse;
                }

                QuietMode = (QuietModeType)quiet;
            }

            return GPErrorCode.NoError;
        }
    }
}
