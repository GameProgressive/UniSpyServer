namespace StatsAndTracking.Entity.Structure
{
    public class GStatsRequestName
    {
        /// <summary>
        /// Authentication user
        /// </summary>
        public const string Auth = "auth";

        /// <summary>
        /// Authenticate player
        /// </summary>
        public const string AuthP = "authp";

        /// <summary>
        /// get player infomation from profileid
        /// </summary>
        public const string GetPid = "getpid";

        /// <summary>
        /// Get player data
        /// </summary>
        public const string GetPD = "getpd";

        /// <summary>
        /// Store player data
        /// </summary>
        public const string SetPD = "setpd";

        /// <summary>
        /// Update a profile data for a game
        /// </summary>
        public const string UpdGame = "updgame";

        /// <summary>
        /// create new player data for a game
        /// </summary>
        public const string NewGame = "newgame";
    }
}
