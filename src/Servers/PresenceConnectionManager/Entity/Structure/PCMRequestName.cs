using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Entity.Structure
{
    class PCMRequestName
    {
        /// <summary>
        /// Log in to PCM server
        /// </summary>
        public const string LogIn = "login";//login to retrospy
        /// <summary>
        /// Logout from PCM server
        /// </summary>
        public const string LogOut = "logout";//logout from retrospy
        /// <summary>
        /// Do not disconnect client from PCM server
        /// </summary>
        public const string KeepAlive = "ka";
        /// <summary>
        /// Get a player's profile
        /// </summary>
        public const string GetPlayerProfile = "getprofile";//get profile of a player
        /// <summary>
        /// Register a nick name for a user
        /// </summary>
        public const string RegisterNickName = "registernick";//update user's uniquenick
        /// <summary>
        /// Create a new user
        /// </summary>
        public const string CreateNewUser = "newuser";//create an new user
        /// <summary>
        /// Update user's information
        /// </summary>
        public const string UpdateUserInformation = "updateui";//update a user's email
        /// <summary>
        /// Update user's profile
        /// </summary>
        public const string UpdateUserProfile = "updatepro";//update a user's profile
        /// <summary>
        /// Create a profile for a user
        /// </summary>
        public const string CreateNewProfile = "newprofile";//create an new profile
        /// <summary>
        /// Delete a user's profile
        /// </summary>
        public const string DeleteProfile = "delprofile";
        /// <summary>
        /// add a user to our block list
        /// </summary>
        public const string AddUserToBlockList = "addblock";
        /// <summary>
        /// Remove a user from blocklist
        /// </summary>
        public const string RemoveUserFromBlockList = "removeblock";
        /// <summary>
        /// Add a user to my friend list
        /// </summary>
        public const string AddUserToFriendList = "addbuddy";//Send a request which adds an user to our friend list
        /// <summary>
        /// Delete a user from my friend list
        /// </summary>
        public const string RemoveUserFromFriendList = "delbuddy";//delete a user from our friend list
        /// <summary>
        /// Update a user's status after login
        /// </summary>
        public const string UpdateUserStatus = "status";//update current logged in user's status info
        /// <summary>
        /// Update a user's status information
        /// </summary>
        public const string UpdateUserStatusInformation = "statusinfo";
        /// <summary>
        /// Invite a user to a room or a game
        /// </summary>
        public const string InviteUserToGame = "inviteto";

    }
}
