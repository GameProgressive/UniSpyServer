using System;
using UniSpy.Server.PresenceConnectionManager.Enumerate;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.PresenceConnectionManager.Aggregate.Misc;
using System.Linq;
using Newtonsoft.Json;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        public int? UserId { get; set; }
        public int? ProfileId { get; set; }
        public int? SubProfileId { get; set; }
        public const ushort SessionKey = 1111;
        public const string LoginTicket = "0000000000000000000000__";
        public DateTime CreatedTime { get; private set; } = DateTime.Now;
        public Guid UserGuid { get; private set; }
        public SdkRevision SdkRevision { get; private set; } = new SdkRevision();
        public UserStatus Status { get; set; } = new UserStatus();
        public UserStatusInfo StatusInfo { get; set; }
        public LoginStatus LoginStat { get; set; } = LoginStatus.Connected;

        [JsonIgnore]
        private User _userInfo;
        [JsonIgnore]
        private Profile _profileInfo;
        [JsonIgnore]
        private Subprofile _subProfileInfo;
        [JsonIgnore]
        public User UserInfo
        {
            get
            {
                if (UserId is null)
                {
                    throw new UniSpy.Exception("UserId is not setted");
                }
                if (_userInfo is null)
                {
                    using (var db = new UniSpyContext())
                    {
                        _userInfo = db.Users.Where(s => s.Userid == UserId).First();
                    }
                }
                return _userInfo;
            }
        }
        [JsonIgnore]
        public Profile ProfileInfo
        {
            get
            {
                if (ProfileId is null)
                {
                    throw new UniSpy.Exception("ProfileId is not set");
                }
                if (_profileInfo is null)
                {
                    using (var db = new UniSpyContext())
                    {
                        _profileInfo = db.Profiles.Where(s => s.Profileid == ProfileId).First();
                    }
                }
                return _profileInfo;
            }
        }
        [JsonIgnore]
        public Subprofile SubProfileInfo
        {
            get
            {
                if (SubProfileId is null)
                {
                    throw new UniSpy.Exception("Subprofile is not set");
                }
                if (_subProfileInfo is null)
                {
                    using (var db = new UniSpyContext())
                    {
                        _subProfileInfo = db.Subprofiles.Where(s => s.Subprofileid == SubProfileId).First();
                    }
                }
                return _subProfileInfo;
            }
        }

        public bool IsRemoteClient { get; set; }
        public ClientInfo() { }
        public ClientInfo DeepCopy()
        {
            var infoCopy = (ClientInfo)this.MemberwiseClone();
            return infoCopy;
        }
    }
}
