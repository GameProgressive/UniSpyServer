using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Structures
{
    public class GPCMPlayerInfo
    {
        /// <summary>
        /// The connected clients Player Id
        /// </summary>
        public uint PlayerId = 0;

        /// <summary>
        /// The connected clients Nick
        /// </summary>
        public string PlayerNick = "";

        /// <summary>
        /// The connected clients Email Address
        /// </summary>
        public string PlayerEmail = "";

        /// <summary>
        /// The connected clients Authentication Token
        /// </summary>
        public string PlayerAuthToken = "";

        /// <summary>
        /// The connected clients Unique Nick
        /// </summary>
        public string PlayerUniqueNick = "";

        /// <summary>
        /// The connected clients country code
        /// </summary>
        public string PlayerCountryCode = "";

        /// <summary>
        /// The clients password, MD5 hashed from UTF8 bytes
        /// </summary>
        public string PasswordHash = "";

        /// <summary>
        /// The clients status
        /// </summary>
        public string PlayerStatusString = "Offline";

        /// <summary>
        /// The place where the client is currently
        /// </summary>
        public string PlayerStatusLocation = "";

        public string PlayerFirstName = "";
        public string PlayerLastName = "";
        public int PlayerICQ = 0;
        public string PlayerHomepage = "";
        public string PlayerZIPCode = "";
        public string PlayerLocation = "";
        public string PlayerAim = "";
        public int PlayerOccupation = 0;
        public int PlayerIndustryID = 0;
        public int PlayerIncomeID = 0;
        public int PlayerMarried = 0;
        public int PlayerChildCount = 0;
        public int PlayerConnectionType = 0;
        public int PlayerPicture = 0;
        public int PlayerInterests = 0;
        public PublicMasks PlayerPublicMask = 0;
        public int PlayerOwnership = 0;
        public ushort PlayerBirthday = 0;
        public ushort PlayerBirthmonth = 0;
        public ushort PlayerBirthyear = 0;
        public PlayerSexType PlayerSex = PlayerSexType.PAT;
        public float PlayerLatitude = 0.0f;
        public float PlayerLongitude = 0.0f;

        /// <summary>
        /// Gets the current status of the player
        /// </summary>
        public PlayerStatus PlayerStatus = PlayerStatus.Offline;

        /// <summary>
        /// Gets the current login status
        /// </summary>
        public LoginStatus LoginStatus = LoginStatus.Connected;

    }
}
