using PresenceConnectionManager.Entity.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Structure
{
    public class UserInfo
    {
        /// <summary>
        /// The connected clients Player Id
        /// </summary>
        public uint ProfileID;

        /// <summary>
        /// User ID in database
        /// </summary>
        public uint UserID;

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

        public uint PartnerID;

        public uint NamespaceID;
        /// <summary>
        /// Gets the current status of the player
        /// </summary>
        public PlayerStatus PlayerStatus = PlayerStatus.Offline;

        public string UserChallenge;

        public ushort SessionKey;

        public LoginStatus LoginStatus;



        public uint SDKRevision;

        public uint ProductID;

        /////////////////////////User status//////////////////////////
        public GPStatus StatusCode;

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

        public string GameName;

        public uint GamePort;

        public uint QuietModeFlag;

        public string LoginTicket = "0000000000000000000000__";
    }
}
