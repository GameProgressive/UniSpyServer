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
                    return "";
                case GPErrorCode.SearchConnectionFailed:
                    return "";
                case GPErrorCode.SearchTimeOut:
                    return "";
                // Check.
                /////////
                case GPErrorCode.Check:
                    return "";
                case GPErrorCode.CheckBadMail:
                    return "";
                case GPErrorCode.CheckBadNick:
                    return "";
                case GPErrorCode.CheckBadPassword:
                    return "";

                // Revoke.
                //////////
                case GPErrorCode.Revoke:
                    return "";
                case GPErrorCode.RevokeNotBuddy:
                    return "";

                // Registeruniquenick.
                //////////////////////
                case GPErrorCode.RegisterUniquenick:
                    return "";
                case GPErrorCode.RegisterUniquenickTaken:
                    return "";
                case GPErrorCode.RegisterUniquenickReserved:
                    return "";
                case GPErrorCode.RegisterUniquenickBadNamespace:
                    return "";

                // Register cdkey.
                //////////////////
                case GPErrorCode.RegisterCDKey:
                    return "";
                case GPErrorCode.RegisterCDKeyBadKey:
                    return "";

                case GPErrorCode.RegisterCDKeyAlreadySet:
                    return "";
                case GPErrorCode.RegisterCDKeyAlreadyTaken:
                    return "";

                default:
                    return "General error.";

            }
        }
    }
}
