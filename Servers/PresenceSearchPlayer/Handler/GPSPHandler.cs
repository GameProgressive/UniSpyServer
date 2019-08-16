using System;
using System.Collections.Generic;
using PresenceSearchPlayer;
using GameSpyLib.Extensions;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer
{
    /// <summary>
    /// This class contians gamespy GPSP functions  which help cdkeyserver to finish the GPSP functionality. 
    /// </summary>
    public class GPSPHandler
    {
        public static GPSPDBQuery DBQuery = null;

        /// <summary>
        /// Get nickname through email and password
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void SearchNicks(GPSPClient client, Dictionary<string, string> dict)
        {
            //Format the password for our database storage
            GPSPHelper.ProessPassword(dict);
            //if not recieved correct request we terminate
            GPErrorCode error = GPSPHelper.IsSearchNicksContianAllKeys(dict);
            if (error!=GPErrorCode.NoError)
            {
                GamespyUtils.SendGPError(client.Stream, (int)error, "Error recieving SearchNicks request.");
                return;
            }

            bool sendUniqueNick = dict.ContainsKey("gamename");

            List<Dictionary<string, object>> queryResult;

            try
            {
                //get nicknames from GPSPDBQuery class
                queryResult = DBQuery.RetriveNicknames(dict);
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Error);
                GamespyUtils.SendGPError(client.Stream,GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
                return;
            }

            if (queryResult.Count < 1)
            {
                GamespyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "No match found !");
               // client.Stream.SendAsync(@"\nr\ndone\final\");
                return;
            }

            string sendingBuffer;
            sendingBuffer = @"\nr\";
            foreach (Dictionary<string, object> row in queryResult)
            {
                sendingBuffer += @"\nick\";
                sendingBuffer += row["nick"];
                if (sendUniqueNick)
                {
                    sendingBuffer += @"\uniquenick\";
                    sendingBuffer += row["uniquenick"];
                }
            }

            sendingBuffer += @"\ndone\final\";
            client.Stream.SendAsync(sendingBuffer);
        }

        public static void IsEmailValid(GPSPClient client, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {
                GamespyUtils.SendGPError(client.Stream, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            try
            {
                if (GamespyUtils.IsEmailFormatCorrect(dict["email"]))
                {
                    if (DBQuery.IsEmailValid(dict["email"]))
                        client.Stream.SendAsync(@"\vr\1\final\");
                    else
                        client.Stream.SendAsync(@"\vr\0\final\");

                    client.Stream.Close();
                }
                else
                {
                    client.Stream.SendAsync(@"\vr\0\final\");
                    client.Stream.Close();
                }

            }
            catch (Exception ex)
            {
                LogWriter.Log.WriteException(ex);
                GamespyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
            }
        }

        /// <summary>
        /// we just simply check the existance of the unique nickname in the database and suggest some numbered postfix nickname
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void SuggestUniqueNickname(GPSPClient client, Dictionary<string, string> dict)
        {
            //The multiple nick suggest correct response is like 
            //@"\us\<number of suggested nick>\nick\<nick1>\nick\<nick2>\usdone\final\";
            string sendingBuffer;
            if (!dict.ContainsKey("preferrednick"))
            {
                GamespyUtils.SendGPError(client.Stream,GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            if (!DBQuery.IsUniqueNickExist(dict["preferrednick"]))
            {
                sendingBuffer = @"\us\1\nick\" + dict["preferrednick"] + @"\usdone\final\";
                client.Stream.SendAsync(sendingBuffer);
            }
            else
            {
                GamespyUtils.SendGPError(client.Stream,GPErrorCode.General, "The Nick is existed, please choose another name");
            }
        }

        public static void SearchProfileWithUniquenick(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("pmatch", dict);
            GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

        public static void OnProfileList(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("profilelist", dict);
            GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

        public static void SeachPlayers(GPSPClient client, Dictionary<string, string> dict)
        {
            string sendingBuffer1, sendingBuffer2;
            sendingBuffer1 = @"\psrdone\";
            sendingBuffer2 = @"\psr\";
            //there are two ways to send information back.

            //First way: \psr\<profileid>\status\<status>\statuscode\<statuscode>\psrdone\final\

            //this is a multiple command. you can contain mutiple \psr\........... in the Steam
            //Second way:\psr\<profileid>\nick\<nick>\***multiple \psr\ command***\psrdone\final\
            //<status> is like the introduction in a player homepage
            //<statuscode> mean the status information is support or not the value should be as follows
            //GP_NEW_STATUS_INFO_SUPPORTED = 0xC00,
            //GP_NEW_STATUS_INFO_NOT_SUPPORTED = 0xC01


            sendingBuffer2 += @"status\";

            GamespyUtils.PrintReceivedGPDictToLogger("pmatch", dict);
            GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

        /// <summary>
        /// Creates an account and use new account to login
        /// </summary>
        /// <param name="client">The client that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        public static void NewUser(GPSPClient client, Dictionary<string, string> dict)
        {
            //Format the password for our database storage
            GPSPHelper.ProessPassword(dict);
            GPErrorCode error = GPSPHelper.IsNewUserContainAllKeys(dict);
            //if there do not recieved right <key,value> pairs we send error
            if (error!=GPErrorCode.NoError)
            {
                GamespyUtils.SendGPError(client.Stream, error, "Error recieving request. Please check the input!");
                return;
            }
            //Check the nick and uniquenick is formated correct and uniquenick is existed in database
            string sendingBuffer;
            error = GPSPHelper.IsEmailNickUniquenickValied(dict,DBQuery);            
            if (error != GPErrorCode.NoError)
            {               
                sendingBuffer = string.Format(@"\nur\{0}\final\", (int)error);
                client.Stream.SendAsync(sendingBuffer);
                return;
            }

            //we get the userid in database. If no userid found according to email we create one 
            //and store the new account into database.
            uint userid = DBQuery.GetuseridFromEmail(dict);
            //create a profile according to userid
            uint pid = DBQuery.CreateUserWithNickOrUniquenick(dict,userid);           
            //Finally we send the create correct to client
            client.Stream.SendAsync(@"\nur\0\pid\{0}\final\", pid);

        }

        public static void SearchOtherBuddyList(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("otherslist", dict);
            GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

        public static void SearchOtherBuddy(GPSPClient client, Dictionary<string, string> dict)
        {


            // TODO: Please finis this function
            string sendingbuffer = @"\otherslist\odone\";
            client.Stream.SendAsync(sendingbuffer);

            //GamespyUtils.PrintReceivedGPDictToLogger("others", dict);
            //GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

        public static void SearchProfile(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("search", dict);
            GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

        public static void CheckProfileId(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("check", dict);
            GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }
    }
}
