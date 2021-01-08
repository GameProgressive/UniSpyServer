using PresenceConnectionManager.Entity.Enumerate;

namespace PresenceConnectionManager.Structure.Data
{
    public class PCMUserInfo
    {
        public uint UserID;
        public uint ProfileID;
        public uint SubProfileID;
        public string Nick;
        public string UniqueNick;
        public string Email;
        public uint PartnerID;
        public uint NamespaceID;
        public uint ProductID;
        public string GameName;
        /// <summary>
        /// The connected clients Authentication Token
        /// </summary>
        public string AuthToken;


        /// <summary>
        /// Gets the current status of the player
        /// </summary>
        public PlayerStatus PlayerStatus = PlayerStatus.Offline;

        public string UserChallenge;

        public const ushort SessionKey = 1111;

        public LoginStatus LoginStatus;

        public SDKRevisionType SDKRevision;

        public uint GamePort;

        public GPBasic QuietModeFlag;

        /////////////////////////User status//////////////////////////
        public GPStatus UserStatus;

        public string StatusString;

        public string LocationString;

        /// <summary>
        /// The profile id parameter that is sent back to the client is initially 2, 
        /// and then 5 everytime after that. So we set here, whether we have sent the 
        /// profile to the client initially (with \id\2) yet.
        /// </summary>
        public bool ProfileSent;

        /// <summary>
        /// This boolean checks if the client has received buddy information
        /// </summary>
        public bool BuddiesSent;

        public bool BlockListSent;



        public const string LoginTicket = "0000000000000000000000__";
    }
}
