using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal class StatusInfoRequest : PCMRequestBase
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

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GPErrorCode.NoError)
            {
                return;
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
                ErrorCode = GPErrorCode.Parse; return;
            }

            StatusState = KeyValues["state"];
            HostIP = KeyValues["hostIp"];
            HostPrivateIP = KeyValues["hprivIp"];

            uint qport;
            if (!uint.TryParse(KeyValues["qport"], out qport))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }
            QueryReportPort = qport;
            uint hport;
            if (uint.TryParse(KeyValues["hport"], out hport))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }
            HostPort = hport;

            uint sessflags;
            if (!uint.TryParse(KeyValues["sessflags"], out sessflags))
            {
                ErrorCode = GPErrorCode.Parse; return;
            }
            SessionFlags = sessflags;

            RichStatus = KeyValues["rechStatus"];
            GameType = KeyValues["gameType"];
            GameVariant = KeyValues["gameVariant"];
            GameMapName = KeyValues["gameMapName"];
        }
    }
}
