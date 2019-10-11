namespace PresenceConnectionManager.Enumerator
{
    /// <summary>
    /// This enum rapresers the known Parter IDs, This value was setted to 0 when
    /// a game is directly connecting to GameSpy, otherwise the Partner ID would be
    /// different for any service that uses GameSpy as backend (for example Nintendo
    /// Wifi Connection).
    /// </summary>
    public enum PartnerID : uint
    {
        /// <summary>
        /// The client is directly connecting to the Server
        /// </summary>
        Gamespy = 0,

        // Unknown Partner ID from 1 to 9
        // EA partner ID should range here, it was

        /// <summary>
        /// Unknown usage for this partner id, but it exists in the
        /// GameSpy SDK
        /// </summary>
        IGN = 10,

        //Nintendo = 11, // Please verify this
    }

    /// <summary>
    /// This enum rapresents the gender of a player.
    /// </summary>
    public enum PlayerSexType : ushort
    {
        /// <summary>
        /// Gender is male
        /// </summary>
        MALE,

        /// <summary>
        /// Gender is female
        /// </summary>
        FEMALE,

        /// <summary>
        /// Unspecified or unknown gender, this is
        /// used to mask the gender when the information is queried
        /// </summary>
        PAT
    }

    /// <summary>
    /// The status of the player
    /// </summary>
    public enum PlayerOnlineStatus : uint
    {
        /// <summary>
        /// The player is offline
        /// </summary>
        Offline = 0,

        /// <summary>
        /// The player is online
        /// </summary>
        Online,

        /// <summary>
        /// The player is playing a game
        /// </summary>
        Playing,

        /// <summary>
        /// Unknown?
        /// </summary>
        Staging,

        /// <summary>
        /// The player is chatting?
        /// </summary>
        Chatting,

        /// <summary>
        /// The player is away from the computer
        /// </summary>
        Away,

        /// <summary>
        /// The player is banned
        /// </summary>
        Banned,
    };
	
	/// <summary>
	/// This enumerator contains the masks used to hide certain informations.
    /// 
    /// The public mask works by ORing this bytes.
    /// 
    /// If the MASK_HOMEPAGE is ORed with MASK_ZIPCODE both the Homepage
    /// and the Zipcode will be showed to the user.
	/// </summary>
    public enum PublicMasks  : uint
    {
        /// <summary>
        /// Show the essential informations for getting the profile info
        /// </summary>
        MASK_NONE = 0x00000000,

        /// <summary>
        /// Show the user homepage
        /// </summary>
        MASK_HOMEPAGE = 0x00000001,

        /// <summary>
        /// Show the ZIP code
        /// </summary>
        MASK_ZIPCODE = 0x00000002,

        /// <summary>
        /// Show the country code where the player lives
        /// </summary>
        MASK_COUNTRYCODE = 0x00000004,

        /// <summary>
        /// Show the birth date
        /// </summary>
        MASK_BIRTHDAY = 0x00000008,

        /// <summary>
        /// Show the gender
        /// </summary>
        MASK_SEX = 0x00000010,

        /// <summary>
        /// Show the Email
        /// </summary>
        MASK_EMAIL = 0x00000020,

        /// <summary>
        /// Show all the informations
        /// </summary>
        MASK_ALL = 0xFFFFFFFF,
    };
}
