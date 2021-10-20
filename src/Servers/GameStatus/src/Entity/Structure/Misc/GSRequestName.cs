namespace UniSpyServer.GameStatus.Entity.Structure.Misc
{
    public sealed class GSRequestName
    {
        /// <summary>
        /// Authenticate user
        /// </summary>
        public const string AuthenticateUser = "auth";

        /// <summary>
        /// Authenticate player
        /// </summary>
        public const string AuthenticatePlayer = "authp";

        /// <summary>
        /// get player infomation from profileid
        /// </summary>
        public const string GetProfileID = "getpid";

        /// <summary>
        /// Get player data
        /// </summary>
        public const string GetPlayerData = "getpd";

        /// <summary>
        /// Store player data
        /// </summary>
        public const string SetPlayerData = "setpd";

        /// <summary>
        /// Update a profile data for a game
        /// </summary>
        public const string UpdateGameData = "updgame";

        /// <summary>
        /// create new player data for a game
        /// </summary>
        public const string CreateNewGamePlayerData = "newgame";
    }
}
