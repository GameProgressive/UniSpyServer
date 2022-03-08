using System;
using System.Net;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Structure.Data
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public const ushort SessionKey = 1111;
        public const string LoginTicket = "0000000000000000000000__";
        public DateTime CreatedTime { get; private set; }
        public Guid UserGuid { get; private set; }
        public SdkRevision SdkRevision { get; private set; }
        public UserStatus Status { get; set; }
        public UserStatusInfo StatusInfo { get; set; }
        public LoginStatus LoginPhase { get; set; }
        public User UserInfo { get; set; }
        public Subprofile SubProfileInfo { get; set; }
        public Profile ProfileInfo { get; set; }
        public ClientInfo(IPEndPoint remoteIPEndPoint) : base(remoteIPEndPoint)
        {
            SdkRevision = new SdkRevision();
            Status = new UserStatus();
            StatusInfo = new UserStatusInfo();
            CreatedTime = DateTime.Now;
            LoginPhase = LoginStatus.Connected;
        }
    }
}
