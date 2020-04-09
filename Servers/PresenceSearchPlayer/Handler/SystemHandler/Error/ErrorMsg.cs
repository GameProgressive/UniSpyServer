using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler.CommandHandler.Error
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
                    return "This request cannot be processed because the session key is invalid.";

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


                // Search.
                //////////
                case GPErrorCode.Search:
                    return "There was an error searching for a profile.";
                case GPErrorCode.SearchConnectionFailed:
                    return "The search attempt failed to connect to the server.";
                case GPErrorCode.SearchTimeOut:
                    return "The search did not return in a timely fashion.";


                // Check.
                /////////
                case GPErrorCode.Check:
                    return "There was an error checking the user account.";
                case GPErrorCode.CheckBadMail:
                    return "No account exists with the provided e-mail address.";
                case GPErrorCode.CheckBadNick:
                    return "No such profile exists for the provided e-mail adress.";
                case GPErrorCode.CheckBadPassword:
                    return "The password is incorrect.";


                // Revoke.
                //////////
                case GPErrorCode.Revoke:
                    return "There was an error revoking the buddy.";
                case GPErrorCode.RevokeNotBuddy:
                    return "You are not a buddy of the profile.";


                // Register cdkey.
                //////////////////
                case GPErrorCode.RegisterCDKey:
                    return "There was an error registering the cdkey.";
                case GPErrorCode.RegisterCDKeyBadKey:
                    return "The cdkey is invalid.";

                case GPErrorCode.RegisterCDKeyAlreadySet:
                    return "The profile has already been registered with a different cdkey.";
                case GPErrorCode.RegisterCDKeyAlreadyTaken:
                    return "The cdkey has already been registered to another profile.";


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
        public static void SendGPSPError(GPSPSession session, GPErrorCode errorCode, uint operationID)
        {
            string errorMsg = ErrorMsg.GetErrorMsg(errorCode);
            string sendingBuffer = string.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\{2}\final\", (uint)errorCode, errorMsg, operationID);
            session.SendAsync(sendingBuffer);
        }
    }
}
