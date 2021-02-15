using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal sealed class StatusInfoRequest : PCMRequestBase
    {
        public bool IsGetStatusInfo { get; set; }
        public string StatusState { get; private set; }
        public string BuddyIP { get; private set; }
        public string HostIP { get; private set; }
        public string HostPrivateIP { get; private set; }
        public uint? QueryReportPort { get; private set; }
        public uint? HostPort { get; private set; }
        public uint? SessionFlags { get; private set; }
        public string RichStatus { get; private set; }
        public string GameType { get; private set; }
        public string GameVariant { get; private set; }
        public string GameMapName { get; private set; }
        public string QuietModeFlags { get; private set; }

        public StatusInfoRequest(string rawRequest) : base(rawRequest)
        {
            IsGetStatusInfo = false;
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
