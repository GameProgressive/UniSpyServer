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
        Male,

        /// <summary>
        /// Gender is female
        /// </summary>
        Female,

        /// <summary>
        /// Unspecified or unknown gender, this is
        /// used to mask the gender when the information is queried
        /// </summary>
        Pat
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
    public enum PublicMasks : uint
    {
        /// <summary>
        /// Show the essential informations for getting the profile info
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Show the user homepage
        /// </summary>
        Homepage = 0x00000001,

        /// <summary>
        /// Show the ZIP code
        /// </summary>
        ZipCode = 0x00000002,

        /// <summary>
        /// Show the country code where the player lives
        /// </summary>
        CountryCode = 0x00000004,

        /// <summary>
        /// Show the birth date
        /// </summary>
        Birthday = 0x00000008,

        /// <summary>
        /// Show the gender
        /// </summary>
        Sex = 0x00000010,

        /// <summary>
        /// Show the Email
        /// </summary>
        Email = 0x00000020,

        /// <summary>
        /// Show all the informations
        /// </summary>
        All = 0xFFFFFFFF,
    };

    /// <summary>
    /// This enumation defins the supported login method for the users.
    /// </summary>
    public enum LoginMethods
    {
        /// <summary>
        /// Login with user combo (nick@email)
        /// </summary>
        Username = 0,

        /// <summary>
        /// Login with unique nickname
        /// </summary>
        UniqueNickname,

        /// <summary>
        /// Pre-authenticated login
        /// </summary>
        AuthToken
    }
}
