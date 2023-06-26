using System;
using UniSpy.Server.PresenceConnectionManager.Enumerate;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.PresenceConnectionManager.Aggregate.Misc;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public const ushort SessionKey = 1111;
        public const string LoginTicket = "0000000000000000000000__";
        public DateTime CreatedTime { get; private set; } = DateTime.Now;
        public Guid UserGuid { get; private set; }
        public SdkRevision SdkRevision { get; private set; } = new SdkRevision();
        public UserStatus Status { get; set; } = new UserStatus();
        public UserStatusInfo StatusInfo { get; set; }
        public LoginStatus LoginStat { get; set; } = LoginStatus.Connected;
        public User UserInfo { get; set; }
        public Subprofile SubProfileInfo { get; set; }
        public Profile ProfileInfo { get; set; }
        public bool IsRemoteClient { get; set; }
        public ClientInfo()
        {
        }
        public ClientInfo DeepCopy()
        {
            var infoCopy = (ClientInfo)this.MemberwiseClone();
            return infoCopy;
        }
    }
}
