namespace PresenceSearchPlayer.Entity.Structure
{
    public class PSPRequestName
    {
        /// <summary>
        /// Search account existance
        /// </summary>
        public const string SearchCmd = "search";
        /// <summary>
        /// Check if the email is existed
        /// </summary>
        public const string ValidCmd = "valid";
        /// <summary>
        /// Search an user with nick name
        /// </summary>
        public const string NickCmd = "nicks";
        public const string PMatchCmd = "pmatch";// get the players match search condition
        public const string CheckCmd = "check"; // check if the account is valid and password is correct
        public const string NewUserCmd = "newuser";//create an new user
        public const string SearchUniqueCmd = "searchunique";//search an user with uniquenick
        public const string OthersCmd = "others";// get the accounts information which match search condition
        public const string OthersListCmd = "otherslist";//search other players friend list to see who is in his list?
        public const string UniqueSearchCmd = "uniquesearch";//search a user with uniquenick and namespaceid
    }
}
