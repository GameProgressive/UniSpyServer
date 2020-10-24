namespace PresenceSearchPlayer.Entity.Structure
{
    public class PSPRequestName
    {
        /// <summary>
        /// Search account existance
        /// </summary>
        public const string SearchAccount = "search";
        /// <summary>
        /// Check if the email is existed
        /// </summary>
        public const string EmailValidCheck = "valid";
        /// <summary>
        /// Search an user with nick name
        /// </summary>
        public const string SearchByNickName = "nicks";
        /// <summary>
        /// get the players match search condition
        /// </summary>
        public const string PlayerSearch = "pmatch";
        /// <summary>
        /// Check if the account is valid and password is correct
        /// </summary>
        public const string CheckAccountValidation = "check";
        /// <summary>
        /// Create a new user
        /// </summary>
        public const string CreateNewUser = "newuser";
        /// <summary>
        /// Search an user with uniquenick
        /// </summary>
        public const string SearchUserByUniqueNickName = "searchunique";
        /// <summary>
        /// Get the accounts information which match search condition
        /// </summary>
        public const string SearchUsersInformation = "others";
        public const string SearchFriendList = "otherslist";//search other players friend list to see who is in his list?
        public const string UniqueSearch = "uniquesearch";//search a user with uniquenick and namespaceid
    }
}
