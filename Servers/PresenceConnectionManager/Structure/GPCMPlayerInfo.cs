using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Structures
{
    public class GPCMPlayerInfo
    {
        /// <summary>
        /// The connected clients Player Id
        /// </summary>
        public uint Profileid = 0;


        public string User = "";
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
        public string UserData;


        /// <summary>
        /// The clients password, MD5 hashed from UTF8 bytes
        /// </summary>
        public string PasswordHash = "";

        /// <summary>
        /// Gets the current status of the player
        /// </summary>
        public PlayerOnlineStatus PlayerStatus = PlayerOnlineStatus.Offline;


        public string UserChallenge;

        public string ServerChallenge;

        public ushort SessionKey;

        public LoginStatus LoginProcess;

        public uint Partnerid;

        public uint Namespaceid;
    }
}
