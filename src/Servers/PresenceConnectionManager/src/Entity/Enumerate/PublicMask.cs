namespace UniSpy.Server.PresenceConnectionManager.Entity.Enumerate
{
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
}
