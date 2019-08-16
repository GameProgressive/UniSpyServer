using GameSpyLib.Common;
using GameSpyLib.Extensions;
using PresenceSearchPlayer.Enumerator;
using GameSpyLib.Database;
using System.Collections.Generic;

namespace PresenceSearchPlayer
{
    public static class GPSPHelper
    {
        public static GPErrorCode IsNewUserContainAllKeys(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("nick"))
            {
                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("email") || !GamespyUtils.IsEmailFormatCorrect(dict["email"]))
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
        /// <summary>
        /// First we need to check the format of email,nick,uniquenick is correct 
        /// and search uniquenick to find if a account is existed
        /// </summary>
        /// <returns></returns>
        public static GPErrorCode IsEmailNickUniquenickValied(Dictionary<string, string> dict, GPSPDBQuery dbquery)
        {
            if (!GamespyUtils.IsNickOrUniquenickFormatCorrect(dict["nick"]))
            {
                return GPErrorCode.NewUserBadNick;
            }
            if (dict["uniquenick"] != "")
            {
                if (!GamespyUtils.IsNickOrUniquenickFormatCorrect(dict["uniquenick"]))
                {
                    return GPErrorCode.NewUserUniquenickInvalid;
                }
                else
                {
                    if (dbquery.IsUniqueNickExist(dict["uniquenick"]))
                    {
                        return GPErrorCode.NewUserUniquenickInUse;
                    }
                }
            }


            return GPErrorCode.NoError;
        }

        public static GPErrorCode IsSearchNicksContianAllKeys(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {

                return GPErrorCode.Parse;
            }

            // First, we try to receive an encoded password
            if (!dict.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!dict.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue                   
                    return GPErrorCode.Parse;
                }
            }
            return GPErrorCode.NoError;
        }

        /// <summary>
        ///  Format the password for our database storage
        /// </summary>
        /// <param name="dict"></param>
        public static void ProessPassword(Dictionary<string, string> dict)
        {
            if (dict.ContainsKey("passenc"))
            {
                //we do nothing with encoded password
                string password;
                password = GamespyUtils.DecodePassword(dict["passenc"]);
                dict["passenc"] = StringExtensions.GetMD5Hash(password);

            }
            else
            {
                string password;
                password = GamespyUtils.DecodePassword(dict["pass"]);
                dict["pass"] = StringExtensions.GetMD5Hash(password);
                dict.Add("passenc", password);
            }
        }
    }
}
