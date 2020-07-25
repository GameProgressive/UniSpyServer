using GameSpyLib.Common.Entity.Interface;
using PresenceConnectionManager.Entity.Enumerator;

namespace PresenceConnectionManager.Handler.Error
{
    public class ErrorMsg
    {
        public static string GetErrorMsg(GPErrorCode errorCode)
        {
            switch (errorCode)
            {
                ///General error message
                case GPErrorCode.General:
                    return "There was an unknown error.";

                case GPErrorCode.Parse:
                    return "There was an error parsing an incoming request.";

                case GPErrorCode.NotLoggedIn:
                    return "This request cannot be processed because you are not logged in.";

                case GPErrorCode.BadSessionKey:
                    return "This request cannot be processed because the _session key is invalid.";

                case GPErrorCode.DatabaseError:
                    return "Can not find information in database";

                case GPErrorCode.Network:
                    return "here was an error connecting a network socket.";

                case GPErrorCode.ForcedDisconnect:
                    return "This profile has been disconnected by another login.";

                case GPErrorCode.ConnectionClose:
                    return "The server has closed the connection";

                case GPErrorCode.UdpLayer:
                    return "There was a problem with the UDP layer.";


                ///Login error message
                case GPErrorCode.Login:
                    return "There was an error logging in to the GP backend.";

                case GPErrorCode.LoginTimeOut:
                    return "The login attempt timed out.";

                case GPErrorCode.LoginBadNick:
                    return "The nickname provided was incorrect.";

                case GPErrorCode.LoginBadEmail:
                    return "The email address provided was incorrect.";

                case GPErrorCode.LoginBadPassword:
                    return "The password provided was incorrect.";

                case GPErrorCode.LoginBadProfile:
                    return "The profile provided was incorrect.";

                case GPErrorCode.LoginProfileDeleted:
                    return "The profile has been deleted.";

                case GPErrorCode.LoginConnectionFailed:
                    return "The server has refused the connection.";

                case GPErrorCode.LoginServerAuthFaild:
                    return "The server could not be authenticated.";

                case GPErrorCode.LoginBadUniquenick:
                    return "The uniquenick provided is incorrect.";

                case GPErrorCode.LoginBadPreAuth:
                    return "There was an error validating the pre-authentication.";

                case GPErrorCode.LoginBadLoginTicket:
                    return "The login ticket was unable to be validated.";

                case GPErrorCode.LoginTicketExpired:
                    return "The login ticket had expired and could not be used.";


                ///New user errors
                case GPErrorCode.NewUser:
                    return "There was an error creating a new user.";

                case GPErrorCode.NewUserBadNick:
                    return "A profile with that nick already exists.";

                case GPErrorCode.NewUserBadPasswords:
                    return "The password does not match the email address.";

                case GPErrorCode.NewUserUniquenickInvalid:
                    return "The uniquenick is invalid.";

                case GPErrorCode.NewUserUniquenickInUse:
                    return "The uniquenick is already in use.";


                ///UpdateUi
                case GPErrorCode.UpdateUI:
                    return "There was an error updating the user information.";

                case GPErrorCode.UpdateUIBadEmail:
                    return "A user with the email adress provided already exists.";


                ///Newprofile
                case GPErrorCode.NewProfile:
                    return "There was an error creating a new profile.";

                case GPErrorCode.NewProfileBadNick:
                    return "The nickname to be replaced does not exist.";

                case GPErrorCode.NewProfileBadOldNick:
                    return "A profile with the nickname provided already exists.";


                ///Updatepro
                case GPErrorCode.UpdatePro:
                    return "There was an error updating the profile information.";

                case GPErrorCode.UpdatePROBadNick:
                    return "A user with the nickname provided already exists.";


                ///Addbuddy
                case GPErrorCode.AddBuddy:
                    return "There was an error adding a buddy.";

                case GPErrorCode.AddBuddyBadForm:
                    return "The profile requesting to add a buddy is invaild.";

                case GPErrorCode.AddBuddyBadNew:
                    return "The profile requested is invalid.";

                case GPErrorCode.AddBuddyAlreadyBuddy:


                //_return "The profile requested is already a buddy.You tried to register a profile id which is already in your friendlist.";
                //_return "The profile requested is on the local profile's block list.";
                //_return "Reserved for future use The profile requested is blocking you.";


                ///Authadd
                case GPErrorCode.AuthAdd:
                    return "There was an error authorizing an add buddy request.";

                case GPErrorCode.AuthAddBadForm:
                    return "The profile being authorized is invalid.";

                case GPErrorCode.AuthAddBadSig:
                    return "The signature for the authorization is invalid.";

                case GPErrorCode.Status:
                    return "There was an error with the status string.";

                case GPErrorCode.Bm:
                    return "There was an error sending a buddy message.";

                case GPErrorCode.BmNotBuddy:
                    return "The profile the message was to be sent to is not a buddy.";

                case GPErrorCode.BmExtInfoNotSupported:
                    return "The profile does not support extended info keys.";

                case GPErrorCode.BmBuddyOffline:
                    return "The buddy to send a message to is offline.";

                case GPErrorCode.GetProfile:
                    return "There was an error getting profile info.";

                case GPErrorCode.GetProfileBadProfile:
                    return "The profile info was requested on is invalid.";

                case GPErrorCode.DelBuddy:
                    return "There was an error deleting the buddy.";

                case GPErrorCode.DelBuddyNotBuddy:
                    return "The buddy to be deleted is not a buddy.";

                case GPErrorCode.DelProfile:
                    return "There was an error deleting the profile.";

                case GPErrorCode.DelProfileLastProfile:
                    return "The last profile cannot be deleted.";


                ///AddBlock
                case GPErrorCode.AddBlock:
                    return "There was an error adding the player to the blocked list.";

                case GPErrorCode.AddBlockAlreadyBlocked:
                    return "The profile specified is already blocked.";


                ///RemoveBlock
                case GPErrorCode.RemoveBlock:
                    return "There was an error removing the player from the blocked list.";

                case GPErrorCode.RemoveBlockNotBlocked:
                    return "The profile specified was not a member of the blocked list.";


                ///Registeruniquenick
                case GPErrorCode.RegisterUniquenick:
                    return "There was an error registering the uniquenick.";
                case GPErrorCode.RegisterUniquenickTaken:
                    return "The uniquenick is already taken.";
                case GPErrorCode.RegisterUniquenickReserved:
                    return "The uniquenick is reserved.";
                case GPErrorCode.RegisterUniquenickBadNamespace:
                    return "Tried to register a nick with no namespace set.";


                default:
                    return "General error.";
            }
        }

        /// <summary>
        /// Send a presence error
        /// </summary>
        /// <param name="client">The stream that will receive the error</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="operationID">The operation id</param>
        public static void SendGPCMError(ISession client, GPErrorCode errorCode, uint operationID)
        {
            string errorMsg = ErrorMsg.GetErrorMsg(errorCode);
            string sendingBuffer = string.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\{2}\final\", (uint)errorCode, errorMsg, operationID);
            client.SendAsync(sendingBuffer);
        }

        public static string BuildGPErrorMsg(GPErrorCode errorCode)
        {
            string errorMsg = GetErrorMsg(errorCode);
            return $@"\error\\err\{ errorCode}\fatal\\errmsg\{errorMsg}";
        }
    }
}
