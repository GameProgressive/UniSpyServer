using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Misc;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Request
{
    internal sealed class StatusInfoRequest : PCMRequestBase
    {
        public bool IsGetStatusInfo { get; set; }

        #region Get buddy status info
        public uint ProfileID { get; set; }
        public uint NameSpaceID { get; set; }
        #endregion
        public PCMUserStatusInfo StatusInfo { get; private set; }

        public StatusInfoRequest()
        {
            IsGetStatusInfo = true;
        }

        public StatusInfoRequest(string rawRequest) : base(rawRequest)
        {
            IsGetStatusInfo = false;
        }

        public override void Parse()
        {
            base.Parse();

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
                throw new GPGeneralException("StatusInfo request is invalid.", GPErrorCode.Parse);
            }

            StatusInfo.StatusState = KeyValues["state"];
            StatusInfo.HostIP = KeyValues["hostIp"];
            StatusInfo.HostPrivateIP = KeyValues["hprivIp"];

            int qport;
            if (!int.TryParse(KeyValues["qport"], out qport))
            {
                throw new GPGeneralException("qport format is incorrect.", GPErrorCode.Parse);
            }
            StatusInfo.QueryReportPort = qport;
            int hport;
            if (int.TryParse(KeyValues["hport"], out hport))
            {
                throw new GPGeneralException("hport format is incorrect.", GPErrorCode.Parse);
            }
            StatusInfo.HostPort = hport;

            uint sessflags;
            if (!uint.TryParse(KeyValues["sessflags"], out sessflags))
            {
                throw new GPGeneralException("sessflags format is incorrect.", GPErrorCode.Parse);
            }
            StatusInfo.SessionFlags = sessflags;

            StatusInfo.RichStatus = KeyValues["rechStatus"];
            StatusInfo.GameType = KeyValues["gameType"];
            StatusInfo.GameVariant = KeyValues["gameVariant"];
            StatusInfo.GameMapName = KeyValues["gameMapName"];
        }
    }
}
