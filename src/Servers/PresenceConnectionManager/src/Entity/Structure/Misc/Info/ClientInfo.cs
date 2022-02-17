using System;
using System.Net;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Structure.Data
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public const ushort SessionKey = 1111;
        public const string LoginTicket = "0000000000000000000000__";

        public ClientInfo(IPEndPoint remoteIPEndPoint) : base(remoteIPEndPoint)
        {
        }

        public DateTime CreatedTime { get; private set; }
        public Guid UserGuid { get; private set; }
        public BasicInfo BasicInfo { get; private set; }
        public SDKRevision SDKRevision { get; private set; }
        public UserStatus Status { get; set; }
        public UserStatusInfo StatusInfo { get; set; }

        // public ClientInfo(Guid guid)
        // {
        //     UserGuid = guid;
        //     CreatedTime = DateTime.Now;
        //     BasicInfo = new BasicInfo();
        //     Status = new UserStatus();
        //     SDKRevision = new SDKRevision();
        //     StatusInfo = new UserStatusInfo();
        // }
    }
}
