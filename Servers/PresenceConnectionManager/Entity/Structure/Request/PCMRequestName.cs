using System;
namespace PresenceConnectionManager.Entity.Structure.Request
{
    public class PCMRequestName
    {
        public const string Login = "login";//login to retrospy
        public const string Logout = "logout";//logout from retrospy
        public const string KeepAlive = "ka";

        public const string GetProfile = "getprofile";//get profile of a player
        public const string RegisterNickname = "registernick";//update user's uniquenick
        public const string CreateNewAccount = "newuser";//create an new user
        public const string UpdateUserInformation = "updateui";//update a user's email
        public const string UpdateProfileInformation = "updatepro";//update a user's profile
        public const string CreateNewProfile = "newprofile";//create an new profile
        public const string DeleteProfile = "delprofile";//delete profile

        public const string AddUserToBlockList = "addblock";//add an user to our block list
        public const string RemoveUserFromBlockList = "removeblock";
        public const string AddUserToFriendList = "addbuddy";//Send a request which adds an user to our friend list
        public const string DeleteUserFromFriendList = "delbuddy";//delete a user from our friend list
        public const string UpdateMyStatus = "status";//update current logged in user's status info
        public const string GetMyStatusInformation = "statusinfo";
        public const string InviteUserToGame = "inviteto";

    }
}
