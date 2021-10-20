﻿using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Misc;
using UniSpyServer.PresenceSearchPlayer.Entity.Exception.General;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Request
{
    /// <summary>
    /// Update a user's status information
    /// </summary>
    [RequestContract("statusinfo")]
    public sealed class StatusInfoRequest : RequestBase
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

            if (!RequestKeyValues.ContainsKey("state")
                || !RequestKeyValues.ContainsKey("hostIp")
                || !RequestKeyValues.ContainsKey("hprivIp")
                || !RequestKeyValues.ContainsKey("qport")
                || !RequestKeyValues.ContainsKey("hport")
                || !RequestKeyValues.ContainsKey("sessflags")
                || !RequestKeyValues.ContainsKey("rechStatus")
                || !RequestKeyValues.ContainsKey("gameType")
                || !RequestKeyValues.ContainsKey("gameVariant")
                || !RequestKeyValues.ContainsKey("gameMapName"))
            {
                throw new GPParseException("StatusInfo request is invalid.");
            }

            StatusInfo.StatusState = RequestKeyValues["state"];
            StatusInfo.HostIP = RequestKeyValues["hostIp"];
            StatusInfo.HostPrivateIP = RequestKeyValues["hprivIp"];

            int qport;
            if (!int.TryParse(RequestKeyValues["qport"], out qport))
            {
                throw new GPParseException("qport format is incorrect.");
            }
            StatusInfo.QueryReportPort = qport;
            int hport;
            if (int.TryParse(RequestKeyValues["hport"], out hport))
            {
                throw new GPParseException("hport format is incorrect.");
            }
            StatusInfo.HostPort = hport;

            uint sessflags;
            if (!uint.TryParse(RequestKeyValues["sessflags"], out sessflags))
            {
                throw new GPParseException("sessflags format is incorrect.");
            }
            StatusInfo.SessionFlags = sessflags;

            StatusInfo.RichStatus = RequestKeyValues["rechStatus"];
            StatusInfo.GameType = RequestKeyValues["gameType"];
            StatusInfo.GameVariant = RequestKeyValues["gameVariant"];
            StatusInfo.GameMapName = RequestKeyValues["gameMapName"];
        }
    }
}
