using GameSpyLib.Common.Entity.Interface;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler.CommandHandler.Error
{
    public class ErrorMsg
    {
        public static string GetErrorMsg(GPError  errorCode)
        {
            switch (errorCode)
            {
                ///General error message
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


                // Search.
                //////////
                case GPError.Search:
                    return "There was an error searching for a profile.";
                case GPError.SearchConnectionFailed:
                    return "The search attempt failed to connect to the server.";
                case GPError.SearchTimeOut:
                    return "The search did not return in a timely fashion.";


                // Check.
                /////////
                case GPError.Check:
                    return "There was an error checking the user account.";
                case GPError.CheckBadMail:
                    return "No account exists with the provided e-mail address.";
                case GPError.CheckBadNick:
                    return "No such profile exists for the provided e-mail adress.";
                case GPError.CheckBadPassword:
                    return "The password is incorrect.";


                // Revoke.
                //////////
                case GPError.Revoke:
                    return "There was an error revoking the buddy.";
                case GPError.RevokeNotBuddy:
                    return "You are not a buddy of the profile.";


                // Register cdkey.
                //////////////////
                case GPError.RegisterCDKey:
                    return "There was an error registering the cdkey.";
                case GPError.RegisterCDKeyBadKey:
                    return "The cdkey is invalid.";

                case GPError.RegisterCDKeyAlreadySet:
                    return "The profile has already been registered with a different cdkey.";
                case GPError.RegisterCDKeyAlreadyTaken:
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
        public static void SendGPSPError(ISession client, GPError  errorCode, uint operationID)
        {
            string errorMsg = GetErrorMsg(errorCode);
            string sendingBuffer = $@"\error\\err\{errorCode}\fatal\\errmsg\{errorMsg}\id\{operationID}\final\";
            client.SendAsync(sendingBuffer);
        }

        public static string BuildGPErrorMsg(GPError  errorCode)
        {
            return $@"\error\\err\{errorCode}\fatal\\errmsg\{GetErrorMsg(errorCode)}\final\";
        }
    }
}
