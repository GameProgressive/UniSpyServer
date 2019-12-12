using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Structure
{
    public class GPCMPlayerInfo
    {
        /// <summary>
        /// The connected clients Player Id
        /// </summary>
        public uint Profileid = 0;

        /// <summary>
        /// The connected clients Nick
        /// </summary>
        public string Nick = "";

        /// <summary>
        /// The connected clients Email Address
        /// </summary>
        public string Email = "";

        /// <summary>
        /// The connected clients Authentication Token
        /// </summary>
        public string AuthToken = "";

        /// <summary>
        /// The connected clients Unique Nick
        /// </summary>
        public string UniqueNick = "";

        /// <summary>
        /// store the information of a user, if using the uniquenick login the userdatawill be his uniquenick if using
        /// authtoken userdata will be authtoken, if using nouniquenick login the userdata will be nick and email.
        /// </summary>
        public string UserData = "";

        /// <summary>
        /// The clients password, MD5 hashed from UTF8 bytes
        /// </summary>
        public string PasswordHash = "";

        /// <summary>
        /// Gets the current status of the player
        /// </summary>
        public PlayerStatus PlayerStatus = PlayerStatus.Offline;


        public string UserChallenge = "";

        public string ServerChallenge = "";

        public ushort SessionKey = 0;

        public LoginStatus LoginProcess;

        public uint Partnerid;

        public uint Namespaceid;

        public uint SDKRevision;

        /// <summary>
        /// The profile id parameter that is sent back to the client is initially 2, 
        /// and then 5 everytime after that. So we set here, whether we have sent the 
        /// profile to the client initially (with \id\2) yet.
        /// </summary>
        public bool ProfileSent = false;

        /// <summary>
        /// This boolean checks if the client has received buddy information
        /// </summary>
        public bool BuddiesSent = false;

        public bool BlockListSent = false;

        public LoginMethods loginMethod;
    }
}
