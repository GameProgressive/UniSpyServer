using System.Collections.Generic;
using GameSpyLib.Logging;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceConnectionManager.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Enumerator;
using Serilog.Events;

namespace PresenceConnectionManager.Entity.Structure.Request.General
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

        public LoginRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("challenge") || !_recv.ContainsKey("response"))
            {
                return GPError.Parse;
            }

            UserChallenge = _recv["challenge"];
            Response = _recv["response"];

            if (_recv.ContainsKey("uniquenick") && _recv.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
                {
                    return GPError.Parse;
                }
                LoginType = LoginType.UniquenickNamespaceID;
                Uniquenick = _recv["uniquenick"];
                UserData = Uniquenick;
                NamespaceID = namespaceID;
            }
            else if (_recv.ContainsKey("authtoken"))
            {
                LoginType = LoginType.AuthToken;
                AuthToken = _recv["authtoken"];
                UserData = AuthToken;
            }
            else if (_recv.ContainsKey("user"))
            {
                LoginType = LoginType.NickEmail;
                UserData = _recv["user"];

                int Pos = UserData.IndexOf('@');
                if (Pos == -1 || Pos < 1 || (Pos + 1) >= UserData.Length)
                {
                    return GPError.Parse;
                }
                Nick = UserData.Substring(0, Pos);
                Email = UserData.Substring(Pos + 1);
            }
            else
            {
                LogWriter.ToLog(LogEventLevel.Error, "Unknown login method detected!");
                return GPError.Parse;
            }


            return ParseOtherData();
        }

        public uint GamePort { get; protected set; }
        public uint PartnerID { get; protected set; }
        public string GameName { get; private set; }
        public QuietModeType QuietMode { get; private set; }

        private GPError ParseOtherData()
        {
            if (_recv.ContainsKey("partnerid"))
            {
                uint partnerID;
                if (!uint.TryParse(_recv["partnerid"], out partnerID))
                {
                    return GPError.Parse;
                }
                PartnerID = partnerID;
            }

            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                uint sdkRevisionType;
                if (!uint.TryParse(_recv["sdkrevision"], out sdkRevisionType))
                {
                    return GPError.Parse;
                }

                SDKType = (SDKRevisionType)sdkRevisionType;
            }

            if (_recv.ContainsKey("gamename"))
            {
                GameName = _recv["gamename"];
            }

            if (_recv.ContainsKey("port"))
            {
                uint gamePort;
                if (!uint.TryParse(_recv["port"], out gamePort))
                {
                    return GPError.Parse;
                }
                GamePort = gamePort;
            }

            if (_recv.ContainsKey("quiet"))
            {
                uint quiet;
                if (!uint.TryParse(_recv["quiet"], out quiet))
                {
                    return GPError.Parse;
                }

                QuietMode = (QuietModeType)quiet;
            }

            return GPError.NoError;
        }
    }
}
