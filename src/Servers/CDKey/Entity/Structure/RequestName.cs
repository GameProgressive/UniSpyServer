namespace CDKey.Entity.Structure
{
    internal class RequestName
    {
        /// <summary>
        /// Reauthenticate with server to determine whether a client is still online
        /// </summary>
        public const string SKey = "skey";
        /// <summary>
        /// Keep alive 
        /// </summary>
        public const string KA = " ka";
        /// <summary>
        /// Authentication CDKey
        /// </summary>
        public const string Auth = "auth";

        /// <summary>
        /// Disconnect from server
        /// </summary>
        public const string Disc = "disc";

        public const string Resp = "resp";

        public const string UON = "uon";
        public const string UOFF = "uoff";
    }
}
