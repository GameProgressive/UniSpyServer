using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Database;
using RetroSpyServer.DBQueries;
using GameSpyLib.Network;
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
        // public DatabaseDriver databaseDriver;
        public GPSPDBQuery DBQuery;
        /// <summary>
        /// Create a dbquery class for handle the database query
        /// </summary>
        /// <param name="dbdriver"></param>
        public GPSPHelper(DatabaseDriver dbdriver)
        {
            DBQuery = new GPSPDBQuery(dbdriver);
        }
        public void RetriveNicknames(TcpStream stream, Dictionary<string, string> dict)
        {
            string password;
            bool sendUniqueNick;

            if (!dict.ContainsKey("email"))
            {
                GamespyUtils.SendGPError(stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            // First, we try to receive an encoded password
            if (!dict.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!dict.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue
                    GamespyUtils.SendGPError(stream, 1, "There was an error parsing an incoming request.");
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
                GamespyUtils.SendGPError(stream, 4, "This request cannot be processed because of a database error.");
                return;
            }

            if (queryResult.Count < 1)
            {
                stream.SendAsync(@"\nr\ndone\final\");
                return;
            }

            // We will recycle the "password" variable by storing the response
            // that we have to send to the stream. This is done for save memory space
            // so we don't have to declare a new variable.

            //password = @"\nr\";
            string sendingBuffer;
            sendingBuffer = @"\nr\";
            foreach (Dictionary<string, object> row in queryResult)
            {
                // password += @"\nick\";
                sendingBuffer += @"\nick\";
                //  password += row["nick"];
                sendingBuffer += row["nick"];
                if (sendUniqueNick)
                {
                    sendingBuffer += @"\uniquenick\";
                    sendingBuffer += row["uniquenick"];
                }
            }

            sendingBuffer += @"\ndone\final\";
            stream.SendAsync(sendingBuffer);
        }
        public void IsEmailValid(TcpStream stream, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {
                GamespyUtils.SendGPError(stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            try
            {
                if (GamespyUtils.IsEmailFormatCorrect(dict["email"]))
                {
                    if (DBQuery.IsEmailValid(dict["email"]))
                        stream.SendAsync(@"\vr\1\final\");
                    stream.Close();
                }
                else
                {
                    stream.SendAsync(@"\vr\0\final\");
                    stream.Close();
                }

            }
            catch (Exception ex)
            {
                LogWriter.Log.WriteException(ex);
                GamespyUtils.SendGPError(stream, 4, "This request cannot be processed because of a database error.");
            }
        }
        public void SuggestUniqueNickname(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("uniquesearch", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        public void OnProfileList(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("profilelist", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        public void MatchProduct(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("pmatch", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        public void CreateUser(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("newuser", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        public void OnOthersList(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("otherslist", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        public void ReverseBuddies(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("others", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");

            // TODO: Please finis this function
            //stream.SendAsync(@"\others\\odone\final\");
        }

        public void SearchUser(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("search", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        public void CheckAccount(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("check", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }
    }
}
