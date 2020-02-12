using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Structure
{
    public class GPCMPlayerInfo
    {
        public ushort OperationID;
        /// <summary>
        /// The connected clients Player Id
        /// </summary>
        public uint Profileid;

        /// <summary>
        /// User ID in database
        /// </summary>
        public uint Userid;
        /// <summary>
        /// The connected clients Nick
        /// </summary>
        public string Nick;

        /// <summary>
        /// The connected clients Email Address
        /// </summary>
        public string Email;

        /// <summary>
        /// The connected clients Authentication Token
        /// </summary>
        public string AuthToken;

        /// <summary>
        /// The connected clients Unique Nick
        /// </summary>
        public string UniqueNick;

        /// <summary>
        /// store the information of a user, if using the uniquenick login the userdatawill be his uniquenick if using
        /// authtoken userdata will be authtoken, if using nouniquenick login the userdata will be nick and email.
        /// </summary>
        public string UserData;

        /// <summary>
        /// The clients password, MD5 hashed from UTF8 bytes
        /// </summary>
        public string PasswordHash;

        /// <summary>
        /// Gets the current status of the player
        /// </summary>
        public PlayerStatus PlayerStatus = PlayerStatus.Offline;


        public string UserChallenge;

        public string ServerChallenge;

        public ushort SessionKey;

        public LoginStatus LoginProcess;

        public uint PartnerID;

        public uint NamespaceID = 0;

        public uint SDKRevision;

        public uint productID;
        /////////////////////////User status//////////////////////////
        public string StatusString;

        public GPStatus StatusCode;

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

        public LoginType LoginType;

        public DisconnectReason DisconReason;

        public string GameName;

        public List<Dictionary<string, object>> FriendList = new List<Dictionary<string, object>>();

        public bool IsEmailVerified;

        public bool IsBlocked;
    }
}
