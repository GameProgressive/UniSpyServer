using System;
using PresenceConnectionManager.Entity.Enumerate;

namespace PresenceConnectionManager.Structure.Data
{
    internal class PCMUserInfo
    {
        public const ushort SessionKey = 1111;
        public const string LoginTicket = "0000000000000000000000__";

        public Guid UserGuid { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public uint UserID;
        public uint ProfileID;
        public uint SubProfileID;
        public string Nick;
        public string UniqueNick;
        public string Email;
        public uint PartnerID;
        public uint NamespaceID;
        public uint ProductID;
        public int? GamePort;
        public string GameName;
        /// <summary>
        /// The connected clients Authentication Token
        /// </summary>
        public string AuthToken;

        /// <summary>
        /// Indicates the user's Login process status.
        /// </summary>
        public LoginStatus LoginStatus;
        public SDKRevisionType SDKRevision;
        public GPBasic QuietModeFlag;

        #region User status
        public GPStatusCode UserCurrentStatus;
        public string StatusString;
        public string LocationString;
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
            UserCurrentStatus = GPStatusCode.Offline;
        }
    }
}
