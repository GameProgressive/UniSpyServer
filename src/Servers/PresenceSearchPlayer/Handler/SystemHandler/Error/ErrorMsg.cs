using UniSpyLib.Abstraction.Interface;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Handler.CommandHandler.Error
{
    public class ErrorMsg
    {
        public static string GetErrorMsg(GPErrorCode errorCode)
        {
            switch (errorCode)
            {
                #region General
                case GPErrorCode.General:
                    return "There was an unknown error.";
                case GPErrorCode.Parse:
                    return "There was an error parsing an incoming request.";
                case GPErrorCode.NotLoggedIn:
                    return "This request cannot be processed because you are not logged in.";
                case GPErrorCode.BadSessionKey:
                    return "This request cannot be processed because the session key is invalid.";
                case GPErrorCode.DatabaseError:
                    return "There is an error in database operation.";
                case GPErrorCode.Network:
                    return "here was an error connecting a network socket.";
                case GPErrorCode.ForcedDisconnect:
                    return "This profile has been disconnected by another login.";
                case GPErrorCode.ConnectionClose:
                    return "The server has closed the connection";
                case GPErrorCode.UdpLayer:
                    return "There was a problem with the UDP layer.";
                #endregion
                #region Login
                case GPErrorCode.Login:
                    return "There was an error logging in to the GP backend.";
                case GPErrorCode.LoginBadEmail:
                    return "The email address provided was incorrect.";
                case GPErrorCode.LoginBadLoginTicket:
                    return "The login ticket was unable to be validated.	";
                case GPErrorCode.LoginBadNick:
                    return "The nickname provided was incorrect.";
                case GPErrorCode.LoginBadPassword:
                    return "The password provided is incorrect.	";
                case GPErrorCode.LoginBadPreAuth:
                    return "There was an error validating the pre-authentication.	";
                case GPErrorCode.LoginBadProfile:
                    return "The profile provided was incorrect.";
                case GPErrorCode.LoginBadUniquenick:
                    return "The uniquenick provided is incorrect.";
                case GPErrorCode.LoginConnectionFailed:
                    return "The server has refused the connection.";
                case GPErrorCode.LoginProfileDeleted:
                    return "The profile has been deleted.";
                case GPErrorCode.LoginServerAuthFaild:
                    return "The server could not be authenticated.";
                case GPErrorCode.LoginTicketExpired:
                    return "The login ticket had expired and could not be used.";
                case GPErrorCode.LoginTimeOut:
                    return "The login attempt timed out.";
                #endregion
                #region Search
                case GPErrorCode.Search:
                    return "There was an error searching for a profile.";
                case GPErrorCode.SearchConnectionFailed:
                    return "The search attempt failed to connect to the server.";
                case GPErrorCode.SearchTimeOut:
                    return "The search did not return in a timely fashion.";
                #endregion

                #region Check
                case GPErrorCode.Check:
                    return "There was an error checking the user account.";
                case GPErrorCode.CheckBadMail:
                    return "No account exists with the provided e-mail address.";
                case GPErrorCode.CheckBadNick:
                    return "No such profile exists for the provided e-mail adress.";
                case GPErrorCode.CheckBadPassword:
                    return "The password is incorrect.";
                #endregion

                #region Revoke
                case GPErrorCode.Revoke:
                    return "There was an error revoking the buddy.";
                case GPErrorCode.RevokeNotBuddy:
                    return "You are not a buddy of the profile.";
                #endregion

                #region Register cdkey 
                case GPErrorCode.RegisterCDKey:
                    return "There was an error registering the cdkey.";
                case GPErrorCode.RegisterCDKeyBadKey:
                    return "The cdkey is invalid.";
                case GPErrorCode.RegisterCDKeyAlreadySet:
                    return "The profile has already been registered with a different cdkey.";
                case GPErrorCode.RegisterCDKeyAlreadyTaken:
                    return "The cdkey has already been registered to another profile.";
                #endregion

                #region NewUser
                case GPErrorCode.NewUser:
                    return "There was an error creating a new user.";
                case GPErrorCode.NewUserBadNick:
                    return "The nick name provided is wrong";
                case GPErrorCode.NewUserBadPasswords:
                    return "The password provided is wrong";
                case GPErrorCode.NewUserUniquenickInUse:
                    return "The uniquenick is already in use.";
                case GPErrorCode.NewUserUniquenickInvalid:
                    return "The uniquenick is invalid.";
                #endregion

                #region Profile
                case GPErrorCode.NewProfile:
                    return "There was an error creating a new profile.";
                case GPErrorCode.NewProfileBadNick:
                    return "The nickname to be replaced does not exist.";
                case GPErrorCode.DelProfile:
                    return "There was an error deleting profile.";
                case GPErrorCode.DelProfileLastProfile:
                    return "The last profile cannot be deleted";
                case GPErrorCode.NewProfileBadOldNick:
                    return "A profile with the nickname provided already exists.";
                case GPErrorCode.UpdatePro:
                    return "There was an error updating the profile information.";
                case GPErrorCode.UpdatePROBadNick:
                    return "A user with the nickname provided already exists.";
                #endregion

                #region Update user infomation
                case GPErrorCode.UpdateUI:
                    return "There was an error updating the user information.";
                case GPErrorCode.UpdateUIBadEmail:
                    return "A user with the email adress provided already exists.	";
                #endregion

                #region Buddy
                case GPErrorCode.AddBuddy:
                    return "There was an error adding a friend.";
                case GPErrorCode.AddBuddyAlreadyBuddy:
                    return "The profile requested is already a friend.";
                case GPErrorCode.AddBuddyBadForm:
                    return "The profile requesting to add a buddy is invaild.";
                case GPErrorCode.AddBuddyBadNew:
                    return "The profile requested is invalid.";
                #endregion
                #region Auth
                case GPErrorCode.AuthAdd:
                    return "There was an error authorizing an add buddy request.";
                case GPErrorCode.AuthAddBadForm:
                    return "The profile being authorized is invalid.";
                case GPErrorCode.AuthAddBadSig:
                    return "The signature for the authorization is invalid.";
                #endregion
                #region Register new uniquenick
                case GPErrorCode.RegisterUniquenick:
                    return "There was an error registering the uniquenick.";
                case GPErrorCode.RegisterUniquenickBadNamespace:
                    return "Tried to register a nick with no namespace set.";
                case GPErrorCode.RegisterUniquenickReserved:
                    return "The uniquenick is reserved.";
                case GPErrorCode.RegisterUniquenickTaken:
                    return "The uniquenick is already taken.";
                #endregion

                case GPErrorCode.AddBlock:
                    return "There was an error adding the player to the blocked list.";
                case GPErrorCode.AddBlockAlreadyBlocked:
                    return "The profile specified is already blocked.";
                case GPErrorCode.RemoveBlock:
                    return "There was an error removing the player from the blocked list.";
                case GPErrorCode.RemoveBlockNotBlocked:
                    return "The profile specified was not a member of the blocked list.";
                default:
                    return "General error.";
            }
        }

        /// <summary>
        /// Send a presence error
        /// </summary>
        /// <param name="session">The stream that will receive the error</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="operationID">The operation id</param>
        public static void SendGPSPError(IUniSpySession session, GPErrorCode errorCode, uint operationID)
        {
            string errorMsg = GetErrorMsg(errorCode);
            string sendingBuffer = $@"\error\\err\{errorCode}\fatal\\errmsg\{errorMsg}\id\{operationID}\final\";
            session.SendAsync(sendingBuffer);
        }

        public static string BuildGPErrorMsg(GPErrorCode errorCode)
        {
            return $@"\error\\err\{(uint)errorCode}\fatal\\errmsg\{GetErrorMsg(errorCode)}\final\";
        }
    }
}
