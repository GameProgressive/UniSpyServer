using UniSpyLib.Abstraction.Interface;
using PresenceSearchPlayer.Entity.Enumerate;

namespace PresenceSearchPlayer.Handler.CommandHandler.Error
{
    public class ErrorMsg
    {
        public static string GetErrorMsg(GPError errorCode)
        {
            switch (errorCode)
            {
                #region General
                case GPError.General:
                    return "There was an unknown error.";
                case GPError.Parse:
                    return "There was an error parsing an incoming request.";
                case GPError.NotLoggedIn:
                    return "This request cannot be processed because you are not logged in.";
                case GPError.BadSessionKey:
                    return "This request cannot be processed because the session key is invalid.";
                case GPError.DatabaseError:
                    return "There is an error in database operation.";
                case GPError.Network:
                    return "here was an error connecting a network socket.";
                case GPError.ForcedDisconnect:
                    return "This profile has been disconnected by another login.";
                case GPError.ConnectionClose:
                    return "The server has closed the connection";
                case GPError.UdpLayer:
                    return "There was a problem with the UDP layer.";
                #endregion
                #region Login
                case GPError.Login:
                    return "There was an error logging in to the GP backend.";
                case GPError.LoginBadEmail:
                    return "The email address provided was incorrect.";
                case GPError.LoginBadLoginTicket:
                    return "The login ticket was unable to be validated.	";
                case GPError.LoginBadNick:
                    return "The nickname provided was incorrect.";
                case GPError.LoginBadPassword:
                    return "The password provided is incorrect.	";
                case GPError.LoginBadPreAuth:
                    return "There was an error validating the pre-authentication.	";
                case GPError.LoginBadProfile:
                    return "The profile provided was incorrect.";
                case GPError.LoginBadUniquenick:
                    return "The uniquenick provided is incorrect.";
                case GPError.LoginConnectionFailed:
                    return "The server has refused the connection.";
                case GPError.LoginProfileDeleted:
                    return "The profile has been deleted.";
                case GPError.LoginServerAuthFaild:
                    return "The server could not be authenticated.";
                case GPError.LoginTicketExpired:
                    return "The login ticket had expired and could not be used.";
                case GPError.LoginTimeOut:
                    return "The login attempt timed out.";
                #endregion
                #region Search
                case GPError.Search:
                    return "There was an error searching for a profile.";
                case GPError.SearchConnectionFailed:
                    return "The search attempt failed to connect to the server.";
                case GPError.SearchTimeOut:
                    return "The search did not return in a timely fashion.";
                #endregion

                #region Check
                case GPError.Check:
                    return "There was an error checking the user account.";
                case GPError.CheckBadMail:
                    return "No account exists with the provided e-mail address.";
                case GPError.CheckBadNick:
                    return "No such profile exists for the provided e-mail adress.";
                case GPError.CheckBadPassword:
                    return "The password is incorrect.";
                #endregion

                #region Revoke
                case GPError.Revoke:
                    return "There was an error revoking the buddy.";
                case GPError.RevokeNotBuddy:
                    return "You are not a buddy of the profile.";
                #endregion

                #region Register cdkey 
                case GPError.RegisterCDKey:
                    return "There was an error registering the cdkey.";
                case GPError.RegisterCDKeyBadKey:
                    return "The cdkey is invalid.";
                case GPError.RegisterCDKeyAlreadySet:
                    return "The profile has already been registered with a different cdkey.";
                case GPError.RegisterCDKeyAlreadyTaken:
                    return "The cdkey has already been registered to another profile.";
                #endregion

                #region NewUser
                case GPError.NewUser:
                    return "There was an error creating a new user.";
                case GPError.NewUserBadNick:
                    return "The nick name provided is wrong";
                case GPError.NewUserBadPasswords:
                    return "The password provided is wrong";
                case GPError.NewUserUniquenickInUse:
                    return "The uniquenick is already in use.";
                case GPError.NewUserUniquenickInvalid:
                    return "The uniquenick is invalid.";
                #endregion

                #region Profile
                case GPError.NewProfile:
                    return "There was an error creating a new profile.";
                case GPError.NewProfileBadNick:
                    return "The nickname to be replaced does not exist.";
                case GPError.DelProfile:
                    return "There was an error deleting profile.";
                case GPError.DelProfileLastProfile:
                    return "The last profile cannot be deleted";
                case GPError.NewProfileBadOldNick:
                    return "A profile with the nickname provided already exists.";
                case GPError.UpdatePro:
                    return "There was an error updating the profile information.";
                case GPError.UpdatePROBadNick:
                    return "A user with the nickname provided already exists.";
                #endregion

                #region Update user infomation
                case GPError.UpdateUI:
                    return "There was an error updating the user information.";
                case GPError.UpdateUIBadEmail:
                    return "A user with the email adress provided already exists.	";
                #endregion

                #region Buddy
                case GPError.AddBuddy:
                    return "There was an error adding a friend.";
                case GPError.AddBuddyAlreadyBuddy:
                    return "The profile requested is already a friend.";
                case GPError.AddBuddyBadForm:
                    return "The profile requesting to add a buddy is invaild.";
                case GPError.AddBuddyBadNew:
                    return "The profile requested is invalid.";
                #endregion
                #region Auth
                case GPError.AuthAdd:
                    return "There was an error authorizing an add buddy request.";
                case GPError.AuthAddBadForm:
                    return "The profile being authorized is invalid.";
                case GPError.AuthAddBadSig:
                    return "The signature for the authorization is invalid.";
                #endregion
                #region Register new uniquenick
                case GPError.RegisterUniquenick:
                    return "There was an error registering the uniquenick.";
                case GPError.RegisterUniquenickBadNamespace:
                    return "Tried to register a nick with no namespace set.";
                case GPError.RegisterUniquenickReserved:
                    return "The uniquenick is reserved.";
                case GPError.RegisterUniquenickTaken:
                    return "The uniquenick is already taken.";
                #endregion

                case GPError.AddBlock:
                    return "There was an error adding the player to the blocked list.";
                case GPError.AddBlockAlreadyBlocked:
                    return "The profile specified is already blocked.";
                case GPError.RemoveBlock:
                    return "There was an error removing the player from the blocked list.";
                case GPError.RemoveBlockNotBlocked:
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
        public static void SendGPSPError(ISession client, GPError errorCode, uint operationID)
        {
            string errorMsg = GetErrorMsg(errorCode);
            string sendingBuffer = $@"\error\\err\{errorCode}\fatal\\errmsg\{errorMsg}\id\{operationID}\final\";
            client.SendAsync(sendingBuffer);
        }

        public static string BuildGPErrorMsg(GPError errorCode)
        {
            return $@"\error\\err\{(uint)errorCode}\fatal\\errmsg\{GetErrorMsg(errorCode)}\final\";
        }
    }
}
