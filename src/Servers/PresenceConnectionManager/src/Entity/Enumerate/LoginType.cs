namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate
{
    /// <summary>
    /// This enumation defins the supported login method for the users.
    /// </summary>
    public enum LoginType : int
    {
        /// <summary>
        /// Login with user combo (nick@email)
        /// </summary>
        NickEmail,

        /// <summary>
        /// Login with unique nickname
        /// </summary>
        UniquenickNamespaceID,

        /// <summary>
        /// Pre-authenticated login
        /// </summary>
        AuthToken
    }
}
