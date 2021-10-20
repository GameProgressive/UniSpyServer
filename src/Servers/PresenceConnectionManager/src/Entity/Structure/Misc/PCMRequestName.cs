namespace UniSpyServer.PresenceConnectionManager.Entity.Structure
{
    public sealed class PCMRequestName
    {
        /// <summary>
        /// Log in to PCM server
        /// </summary>
        public const string Login = "login";
        /// <summary>
        /// Logout from PCM server
        /// </summary>
        public const string Logout = "logout";
        /// <summary>
        /// Do not disconnect client from PCM server
        /// </summary>
        public const string KeepAlive = "ka";
        /// <summary>
        /// Get a player's profile
        /// </summary>
        public const string GetProfile = "getprofile";
        /// <summary>
        /// Register a nick name for a user
        /// </summary>
        public const string RegisterNick = "registernick";
        /// <summary>
        /// Create a new user
        /// </summary>
        public const string NewUser = "newuser";
        /// <summary>
        /// Update user's information
        /// </summary>
        public const string UpdateUI = "updateui";
        /// <summary>
        /// Update user's profile
        /// </summary>
        public const string UpdatePro = "updatepro";
        /// <summary>
        /// Create a profile for a user
        /// </summary>
        public const string NewProfile = "newprofile";
        /// <summary>
        /// Delete a user's profile
        /// </summary>
        public const string DelProfile = "delprofile";
        /// <summary>
        ///Add a user to our block list
        /// </summary>
        public const string AddBlock = "addblock";
        /// <summary>
        /// Remove a user from blocklist
        /// </summary>
        public const string RemoveBlock = "removeblock";
        /// <summary>
        /// Add a user to my friend list
        /// </summary>
    }
}
