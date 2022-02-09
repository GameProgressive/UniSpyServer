using System;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc;

namespace UniSpyServer.Servers.PresenceConnectionManager.Structure.Data
{
    public sealed class UserInfo
    {
        public const ushort SessionKey = 1111;
        public const string LoginTicket = "0000000000000000000000__";
        public DateTime CreatedTime { get; private set; }
        public Guid UserGuid { get; private set; }
        public BasicInfo BasicInfo { get; private set; }
        public SDKRevision SDKRevision { get; private set; }
        public UserStatus Status { get; set; }
        public UserStatusInfo StatusInfo { get; set; }

        public UserInfo(Guid guid)
        {
            UserGuid = guid;
            CreatedTime = DateTime.Now;
            BasicInfo = new BasicInfo();
            Status = new UserStatus();
            SDKRevision = new SDKRevision();
            StatusInfo = new UserStatusInfo();
        }
    }
}
