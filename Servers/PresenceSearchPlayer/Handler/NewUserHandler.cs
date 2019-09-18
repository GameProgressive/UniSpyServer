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
            GPSPHandler.ProessPassword(dict);
            GPErrorCode error = IsNewUserContainAllKeys(dict);
            //if there do not recieved right <key,value> pairs we send error
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(client.Stream, error, "Error recieving request. Please check the input!");
                return;
            }
            //Check the nick and uniquenick is formated correct and uniquenick is existed in database
            string sendingBuffer;
            error = IsEmailNickUniquenickValied(dict, GPSPHandler.DBQuery);
            if (error != GPErrorCode.NoError)
            {
                sendingBuffer = string.Format(@"\nur\{0}\final\", (int)error);
                client.Stream.SendAsync(sendingBuffer);
                return;
            }

            //we get the userid in database. If no userid found according to email we create one 
            //and store the new account into database.
            
            if (dict["uniquenick"] == "")
            {
                CreateUserWithoutUniquenick(dict, client);
            }
            else
            {
                CreateUserWithUniquenick(dict, client);
            }
        }


        public static GPErrorCode IsNewUserContainAllKeys(Dictionary<string, string> dict)
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
            if (!GameSpyUtils.IsNickOrUniquenickFormatCorrect(dict["nick"]))
            {
                return GPErrorCode.NewUserBadNick;
            }
            if (dict["uniquenick"] != "")
            {
                if (!GameSpyUtils.IsNickOrUniquenickFormatCorrect(dict["uniquenick"]))
                {
                    return GPErrorCode.NewUserUniquenickInvalid;
                }
                else
                {
                    if (NewUserQuery.IsUniqueNickExistForNewUser(dict))
                    {
                        return GPErrorCode.NewUserUniquenickInUse;
                    }
                }
            }


            return GPErrorCode.NoError;
        }


        public static void CreateUserWithUniquenick(Dictionary<string,string>dict,GPSPClient client)
        {
            uint userid = NewUserQuery.GetUseridFromEmail(dict);
            //create a profile according to userid
            uint pid;// profileid in database
           Dictionary<string, object> queryResult;
            queryResult = NewUserQuery.CreateUserWithNick(dict, userid);
            //if user's nick name is exist we can not continue;
            if (queryResult == null)
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "Nick is registered, please use another one.");
            }
            else
            {
                pid = (uint)queryResult["profileid"];
                client.Stream.SendAsync(@"\nur\0\pid\{0}\final\", pid);
            }
        }

        public static void CreateUserWithoutUniquenick(Dictionary<string, string> dict, GPSPClient client)
        {
            uint userid = NewUserQuery.GetUseridFromEmail(dict);
            //create a profile according to userid
            uint pid;// profileid in database
            Dictionary<string, object> queryResult;

            queryResult = NewUserQuery.CreateUserWithNick(dict, userid);
            //if user's information is exist we can not continue;
            if (queryResult == null)
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "Nick or uniquenick is registered, please use another one.");
            }
            else
            {
                pid = (uint)queryResult["profileid"];
                string message = string.Format("User created pid:{0} but missing unique nickname, please go to rspy.org update uniquenick", pid);
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.NewUserUniquenickInvalid, message);
            }
        }
    }
}
