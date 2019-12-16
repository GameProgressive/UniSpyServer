using System;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler.Error
{
    public class GPSPErrorCode
    {
        public static string GetErrorMsg(GPErrorCode errorCode)
        {
            switch (errorCode)
            {
                // Search.
                //////////
                case GPErrorCode.Search:
                    return "There was an error searching for a profile.";
                case GPErrorCode.SearchConnectionFailed:
                    return "The search attempt failed to connect to the server.	";
                case GPErrorCode.SearchTimeOut:
                    return "The search did not return in a timely fashion.	";
                // Check.
                /////////
                case GPErrorCode.Check:
                    return "There was an error checking the user account.";
                case GPErrorCode.CheckBadMail:
                    return "No account exists with the provided e-mail address.";
                case GPErrorCode.CheckBadNick:
                    return "No such profile exists for the provided e-mail adress.	";
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
    }
}
