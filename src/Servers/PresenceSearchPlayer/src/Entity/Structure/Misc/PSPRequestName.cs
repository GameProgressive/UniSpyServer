namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure
{
    public class PSPRequestName
    {
        /// <summary>
        /// Search account existance
        /// </summary>
        public const string Search = "search";
        /// <summary>
        /// Check if the email is existed
        /// </summary>
        public const string Valid = "valid";
        /// <summary>
        /// Search an user with nick name
        /// </summary>
        public const string Nicks = "nicks";
        /// <summary>
        /// get the players match search condition
        /// </summary>
        public const string PMatch = "pmatch";
        /// <summary>
        /// Check if the account is valid and password is correct
        /// </summary>
        public const string Check = "check";
        /// <summary>
        /// Create a new user
        /// </summary>
        public const string NewUser = "newuser";
        /// <summary>
        /// Search an user with uniquenick
        /// </summary>
        public const string SearchUnique = "searchunique";
        /// <summary>
        /// Get the accounts information which match search condition
        /// </summary>
        public const string Others = "others";
        /// <summary>
        /// search other players friend list to see who is in his list?
        /// </summary>
        public const string OtherList = "otherslist";
        /// <summary>
        /// search a user with uniquenick and namespaceid
        /// </summary>
        public const string UniqueSearch = "uniquesearch";
    }
}
