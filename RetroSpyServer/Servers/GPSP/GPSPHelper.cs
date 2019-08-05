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
        public static GPErrorCode IsCreateUserContainAllKeys(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("nick"))
            {
                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("email"))
            {
                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("passenc"))
            {
                if (!dict.ContainsKey("pass"))
                {
                    return GPErrorCode.Parse;
                }
            }

            if (!dict.ContainsKey("productID"))
            {
                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("namespaceid"))
            {
                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("uniquenick"))
            {
                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("partnerid"))
            {

                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("gamename"))
            {
                return GPErrorCode.Parse;
            }
            return GPErrorCode.NoError;
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
