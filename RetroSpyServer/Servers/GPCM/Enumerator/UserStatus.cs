namespace RetroSpyServer.Servers.GPCM.Enumerator
{
    public enum UserStatus : int
    {
        /// <summary>
        /// The user is created, but not verified
        /// </summary>
        Created,
        /// <summary>
        /// The user is verified, and can login
        /// </summary>
        Verified,
        /// <summary>
        /// The user id banned
        /// </summary>
        Banned
    }
}
