using System;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Misc;

namespace PresenceConnectionManager.Structure.Data
{
    internal class PCMUserInfo
    {
        public const ushort SessionKey = 1111;
        public const string LoginTicket = "0000000000000000000000__";

        public DateTime CreatedTime { get; private set; }
        public Guid UserGuid { get; private set; }
        public PCMBasicInfo BasicInfo { get; private set; }
        public PCMSDKRevision SDKRevision { get; private set; }

        #region User status
        public PCMUserStatus UserStatus { get; private set; }
        public PCMUserStatusInfo UserStatusInfo { get; private set; }
        #endregion

        /// <summary>
        /// The profile id parameter that is sent back to the client is initially 2, 
        /// and then 5 everytime after that. So we set here, whether we have sent the 
        /// profile to the client initially (with \id\2) yet.
        /// </summary>
        public bool ProfileSent;

        public PCMUserInfo(Guid guid)
        {
            UserGuid = guid;
            CreatedTime = DateTime.Now;
            BasicInfo = new PCMBasicInfo();
            UserStatus = new PCMUserStatus();
            SDKRevision = new PCMSDKRevision();
            UserStatusInfo = new PCMUserStatusInfo();
        }
    }
}
