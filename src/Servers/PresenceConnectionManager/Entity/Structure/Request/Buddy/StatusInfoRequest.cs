using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    public class StatusInfoRequest : PCMRequestBase
    {
        public string StatusState { get; protected set; }
        public string BuddyIP { get; protected set; }
        public string HostIP { get; protected set; }
        public string HostPrivateIP { get; protected set; }
        public uint? QueryReportPort { get; protected set; }
        public uint? HostPort { get; protected set; }
        public uint? SessionFlags { get; protected set; }
        public string RichStatus { get; protected set; }
        public string GameType { get; protected set; }
        public string GameVariant { get; protected set; }
        public string GameMapName { get; protected set; }
        public string QuietModeFlags { get; protected set; }

        public StatusInfoRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }
            if (!KeyValues.ContainsKey("state")
                || !KeyValues.ContainsKey("hostIp")
                || !KeyValues.ContainsKey("hprivIp")
                || !KeyValues.ContainsKey("qport")
                || !KeyValues.ContainsKey("hport")
                || !KeyValues.ContainsKey("sessflags")
                || !KeyValues.ContainsKey("rechStatus")
                || !KeyValues.ContainsKey("gameType")
                || !KeyValues.ContainsKey("gameVariant")
                || !KeyValues.ContainsKey("gameMapName"))
            {
                return GPErrorCode.Parse;
            }

            StatusState = KeyValues["state"];
            HostIP = KeyValues["hostIp"];
            HostPrivateIP = KeyValues["hprivIp"];

            uint qport;
            if (!uint.TryParse(KeyValues["qport"], out qport))
            {
                return GPErrorCode.Parse;
            }
            QueryReportPort = qport;
            uint hport;
            if (uint.TryParse(KeyValues["hport"], out hport))
            {
                return GPErrorCode.Parse;
            }
            HostPort = hport;

            uint sessflags;
            if (!uint.TryParse(KeyValues["sessflags"], out sessflags))
            {
                return GPErrorCode.Parse;
            }
            SessionFlags = sessflags;

            RichStatus = KeyValues["rechStatus"];
            GameType = KeyValues["gameType"];
            GameVariant = KeyValues["gameVariant"];
            GameMapName = KeyValues["gameMapName"];

            return GPErrorCode.NoError;
        }
    }
}
