using System;
using System.Collections.Generic;
using RetroSpyServer.DBQueries;
using GameSpyLib.Extensions;
using GameSpyLib.Common;
using GameSpyLib.Logging;

namespace RetroSpyServer.Servers.GPSP
{
    /// <summary>
    /// This class contians gamespy GPSP functions  which help cdkeyserver to finish the GPSP functionality. 
    /// </summary>
    public class GPSPHelper
    {
        public static GPSPDBQuery DBQuery = null;

        public static void RetriveNicknames(GPSPClient client, Dictionary<string, string> dict)
        {
            string password;
            bool sendUniqueNick;

            if (!dict.ContainsKey("email"))
            {
                GamespyUtils.SendGPError(client.Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            // First, we try to receive an encoded password
            if (!dict.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!dict.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue
                    GamespyUtils.SendGPError(client.Stream, 1, "There was an error parsing an incoming request.");
                    return;
                }
                else
                {
                    // Store the plain text password
                    password = dict["pass"];
                }
            }
            else
            {
                // Store the decrypted password
                password = GamespyUtils.DecodePassword(dict["passenc"]);
            }

            password = StringExtensions.GetMD5Hash(password);

            sendUniqueNick = dict.ContainsKey("gamename");

            List<Dictionary<string, object>> queryResult;

            try
            {
                //get nicknames from GPSPDBQuery class
                queryResult = DBQuery.RetriveNicknames(dict["email"], password);
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Error);
                GamespyUtils.SendGPError(client.Stream, 4, "This request cannot be processed because of a database error.");
                return;
            }

            if (queryResult.Count < 1)
            {
                client.Stream.SendAsync(@"\nr\ndone\final\");
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
                GamespyUtils.SendGPError(client.Stream, 1, "There was an error parsing an incoming request.");
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
                GamespyUtils.SendGPError(client.Stream, 4, "This request cannot be processed because of a database error.");
            }
        }

        public static void SuggestUniqueNickname(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("uniquesearch", dict);
            GamespyUtils.SendGPError(client.Stream, 0, "This request is not supported yet.");
        }

        public static void OnProfileList(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("profilelist", dict);
            GamespyUtils.SendGPError(client.Stream, 0, "This request is not supported yet.");
        }

        public static void MatchProduct(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("pmatch", dict);
            GamespyUtils.SendGPError(client.Stream, 0, "This request is not supported yet.");
        }

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="client">The client that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        public static void CreateUser(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("newuser", dict);
            GamespyUtils.SendGPError(client.Stream, 0, "This request is not supported yet.");
        }

        public static void OnOthersList(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("otherslist", dict);
            GamespyUtils.SendGPError(client.Stream, 0, "This request is not supported yet.");
        }

        public static void ReverseBuddies(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("others", dict);
            GamespyUtils.SendGPError(client.Stream, 0, "This request is not supported yet.");

            // TODO: Please finis this function
            //stream.SendAsync(@"\others\\odone\final\");
        }

        public static void SearchUser(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("search", dict);
            GamespyUtils.SendGPError(client.Stream, 0, "This request is not supported yet.");
        }

        public static void CheckAccount(GPSPClient client, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("check", dict);
            GamespyUtils.SendGPError(client.Stream, 0, "This request is not supported yet.");
        }
    }
}
