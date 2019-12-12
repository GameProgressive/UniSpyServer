using PresenceConnectionManager.Enumerator;


namespace PresenceConnectionManager.Handler
{
    public class GPCMErrorMsg
    {
        private static string _message;
        public static string GetErrorMsg(GPErrorCode errorCode)
        {
            switch (errorCode)
            {
                ///General error message
                case GPErrorCode.General:
                    _message = "There was an unknown error.";
                    break;
                case GPErrorCode.Parse:
                    _message = "There was an error parsing an incoming request.";
                    break;
                case GPErrorCode.NotLoggedIn:
                    _message = "This request cannot be processed because you are not logged in.";
                    break;
                case GPErrorCode.BadSessionKey:
                    _message = "This request cannot be processed because the session key is invalid.";
                    break;
                case GPErrorCode.DatabaseError:
                    _message = "Can not find information in database";
                    break;
                case GPErrorCode.Network:
                    _message = "here was an error connecting a network socket.";
                    break;
                case GPErrorCode.ForcedDisconnect:
                    _message = "This profile has been disconnected by another login.";
                    break;
                case GPErrorCode.ConnectionClose:
                    _message = "The server has closed the connection";
                    break;
                case GPErrorCode.UdpLayer:
                    _message = "There was a problem with the UDP layer.";
                    break;


                ///Login error message
                case GPErrorCode.Login:
                    _message = "There was an error logging in to the GP backend.";
                    break;
                case GPErrorCode.LoginTimeOut:
                    _message = "The login attempt timed out.";
                    break;
                case GPErrorCode.LoginBadNick:
                    _message = "The nickname provided was incorrect.";
                    break;
                case GPErrorCode.LoginBadEmail:
                    _message = "The email address provided was incorrect.";
                    break;
                case GPErrorCode.LoginBadPassword:
                    _message = "The password provided was incorrect.";
                    break;
                case GPErrorCode.LoginBadProfile:
                    _message = "The profile provided was incorrect.";
                    break;
                case GPErrorCode.LoginProfileDeleted:
                    _message = "The profile has been deleted.";
                    break;
                case GPErrorCode.LoginConnectionFailed:
                    _message = "The server has refused the connection.";
                    break;
                case GPErrorCode.LoginServerAuthFaild:
                    _message = "The server could not be authenticated.";
                    break;
                case GPErrorCode.LoginBadUniquenick:
                    _message = "The uniquenick provided is incorrect.";
                    break;
                case GPErrorCode.LoginBadPreAuth:
                    _message = "There was an error validating the pre-authentication.";
                    break;
                case GPErrorCode.LoginBadLoginTicket:
                    _message = "The login ticket was unable to be validated.";
                    break;
                case GPErrorCode.LoginTicketExpired:
                    _message = "The login ticket had expired and could not be used.";
                    break;

                ///New user errors
                case GPErrorCode.NewUser:
                    _message = "There was an error creating a new user.";
                    break;
                case GPErrorCode.NewUserBadNick:
                    _message = "A profile with that nick already exists.";
                    break;
                case GPErrorCode.NewUserBadPasswords:
                    _message = "The password does not match the email address.";
                    break;
                case GPErrorCode.NewUserUniquenickInvalid:
                    _message = "The uniquenick is invalid.";
                    break;
                case GPErrorCode.NewUserUniquenickInUse:
                    _message = "The uniquenick is already in use.";
                    break;



            }
            return _message;
        }
    }
}
