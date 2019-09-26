using GameSpyLib.Common;
using PresenceSearchPlayer.DatabaseQuery;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    public class NewUserHandler
    {
        /// <summary>
        /// Creates an account and use new account to login
        /// </summary>
        /// <param name="client">The client that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        public static void NewUser(GPSPClient client, Dictionary<string, string> dict)
        {
            //Format the password for our database storage           
            GPErrorCode error = IsContainAllKeys(dict);
            //if there do not recieved right <key,value> pairs we send error
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(client.Stream, error, "Error recieving request. Please check the input!");
                return;
            }
            //Check the nick and uniquenick is formated correct and uniquenick is existed in database
            string sendingBuffer;
            PreProcessRequest(dict);
            error = IsEmailNickUniquenickValied(dict);
            if (error != GPErrorCode.NoError)
            {
                sendingBuffer = string.Format(@"\nur\{0}\final\", (int)error);
                client.Stream.SendAsync(sendingBuffer);
                return;
            }

            //if the request did not contain uniquenick and namespaceid we use our way to create it.

            //we get the userid in database. If no userid found according to email we create one 
            //and store the new account into database.
            int profileid = CreateAccount(dict);

            if (profileid == -1)
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "Account is existed, please use another one.");
            }
            else
            {
                client.Stream.SendAsync(@"\nur\0\pid\{0}\final\", profileid);
            }

        }


        public static GPErrorCode IsContainAllKeys(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("nick"))
            {
                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("email") || !GameSpyUtils.IsEmailFormatCorrect(dict["email"]))
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

            if (!dict.ContainsKey("productid"))
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
        public static GPErrorCode IsEmailNickUniquenickValied(Dictionary<string, string> dict)
        {
            if (!GameSpyUtils.IsNickOrUniquenickFormatCorrect(dict["nick"]))
            {
                return GPErrorCode.NewUserBadNick;
            }
            if (dict.ContainsKey("uniquenick")&&dict["uniquenick"]!="")
            {
                if (!GameSpyUtils.IsNickOrUniquenickFormatCorrect(dict["uniquenick"]))
                {
                    return GPErrorCode.NewUserUniquenickInUse;
                }
            }
            return GPErrorCode.NoError;
        }

        public static void PreProcessRequest(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("uniquenick"))
            {
                dict.Add("uniquenick", CreateUniquenickInRequest(dict));
            }
            else if (dict["uniquenick"] == "")
            {
                dict["uniquenick"] = CreateUniquenickInRequest(dict);
            }
            if (!dict.ContainsKey("namespaceid"))
            {
                dict.Add("namespaceid", "0");
            }
        }
        public static string CreateUniquenickInRequest(Dictionary<string, string> dict)
        {
            string user = dict["email"];
            int Pos = user.IndexOf('@');
            //we add the nick and email to dictionary
            string uniquenick = user.Substring(0, Pos);
            return uniquenick;
        }

        /// <summary>
        /// create account on our database
        /// </summary>
        /// <param name="dict"></param>
        /// <returns>return profileid, if profile exsit returns -1</returns>
        public static int CreateAccount(Dictionary<string, string> dict)
        {

            //check is user exist in users table, if not exist we create
            int userid = NewUserQuery.CreateUserOnTableUsers(dict);

            //find or create a profile according to userid          
            int profileid = NewUserQuery.CreateProfileOnTableProfiles(dict, userid);

            bool IsCreationSuccess = NewUserQuery.CreateSubprofileOnTableNamespace(dict, profileid);

            if (IsCreationSuccess)
            {
                return profileid;
            }
            else
            {
                return -1;
            }
        }
    }
}
