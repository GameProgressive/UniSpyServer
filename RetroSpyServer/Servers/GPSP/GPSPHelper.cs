using GameSpyLib.Common;
using GameSpyLib.Extensions;
using RetroSpyServer.Servers.GPSP.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.GPSP
{
    public static class GPSPHelper
    {
        public static bool IsCreateUserContainAllKeys(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("nick"))
            {
                return false;
            }
            if (!dict.ContainsKey("email"))
            {
                return false;
            }
            if (!dict.ContainsKey("passenc"))
            {
                if (!dict.ContainsKey("pass"))
                {
                    return false;
                }
            }

            if (!dict.ContainsKey("productID"))
            {
                return false;
            }
            if (!dict.ContainsKey("namespaceid"))
            {
                return false;
            }
            if (!dict.ContainsKey("uniquenick"))
            {
                return false;
            }
            if (!dict.ContainsKey("partnerid"))
            {

                return false;
            }
            if (!dict.ContainsKey("gamename"))
            {
                return false;
            }
            return true;
        }
        public static GPErrorCode IsSearchNicksContianAllKeys(Dictionary<string, string> dict,out string password)
        {
            if (!dict.ContainsKey("email"))
            {
                password = null;
                    return GPErrorCode.Parse;
            }

            // First, we try to receive an encoded password
            if (!dict.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!dict.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue  
                    password = null;
                    return GPErrorCode.Parse;
                }
                else
                {
                    password = GamespyUtils.DecodePassword(dict["pass"]);
                    // encrypt password
                    password = StringExtensions.GetMD5Hash(password);
                    return GPErrorCode.NoError;
                }
            }
            else
            {

                // Store the decrypted password
                password = GamespyUtils.DecodePassword(dict["passenc"]);
                password = StringExtensions.GetMD5Hash(password);
                //password = dict["passenc"];
                return GPErrorCode.NoError;
            }
        }
    }



}
