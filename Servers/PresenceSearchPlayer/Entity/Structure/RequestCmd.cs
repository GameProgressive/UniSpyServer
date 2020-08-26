using System;
namespace PresenceSearchPlayer.Entity.Structure
{
    public class RequestCmd
    {
        public const string SearchCmd = "search";
        public const string ValidCmd = "valid";//is email format valid
        public const string NickCmd = "nicks";// search an user with nick name
        public const string PMatchCmd = "pmatch";
        public const string CheckCmd = "check";
        public const string NewUserCmd = "newuser";//create an new user
        public const string SearchUniqueCmd = "searchunique";//search an user with uniquenick
        public const string OthersCmd = "others";//search 
        public const string OthersListCmd = "otherslist";//search other players friend list to see who is in his list?
        public const string UniqueSearchCmd = "uniquesearch";//search a user with uniquenick and namespaceid
    }
}
