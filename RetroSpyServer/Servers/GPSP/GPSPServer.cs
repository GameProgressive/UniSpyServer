using System;
using System.Collections.Generic;
using System.Net;
using GameSpyLib.Common;
using GameSpyLib.Database;
using GameSpyLib.Network;
using GameSpyLib.Logging;
using GameSpyLib.Extensions;
using RetroSpyServer.Extensions;

namespace RetroSpyServer.Servers.GPSP
{
    public class GPSPServer : GamespyTcpSocket
    {
        private DatabaseDriver databaseDriver = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// If the databaseDriver is null, then the server will attempt to create it's own connection
        /// otherwise it will use the specified connection
        /// </param>
        public GPSPServer(DatabaseDriver databaseDriver, IPEndPoint bindTo, int MaxConnections) : base(bindTo, MaxConnections)
        {
            if (databaseDriver == null)
                this.databaseDriver = DatabaseUtility.CreateNewMySQLConnection();
            else
                this.databaseDriver = databaseDriver;

            // Begin accepting connections
            StartAcceptAsync();
        }

        ~GPSPServer()
        {
            Shutdown();
        }

        public void Shutdown()
        {
            // Stop accepting new connections
            IgnoreNewConnections = true;

            // Shutdown the listener socket
            ShutdownSocket();

            // Tell the base to dispose all free objects
            Dispose();
        }

        /// <summary>
        /// This function is fired when an exception occour in the server
        /// </summary>
        /// <param name="e">The exception to be thrown</param>
        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

        /// <summary>
        /// This function is fired when a client is being accepted
        /// </summary>
        /// <param name="Stream">The stream of the client to be accepted</param>
        protected override void ProcessAccept(GamespyTcpStream Stream)
        {
            Stream.DataReceived += ProcessDataReceived;
            Stream.OnDisconnect += (stream) => stream.Dispose();
            Stream.BeginReceive();
        }

        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected void ProcessDataReceived(GamespyTcpStream stream, string message)
        {
            if (!message.EndsWith("\\final\\") || message[0] != '\\')
            {
                GamespyUtils.SendGPError(stream, 0, "An invalid request was sended.");
                stream.Close();
                Release(stream);
                return;
            }

            string[] recieved = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = GamespyUtils.ConvertGPResponseToKeyValue(recieved);

            switch (recieved[0])
            {
                case "valid":
                    IsEmailValid(stream, dict);
                    break;
                case "nicks":
                    RetriveNicknames(stream, dict);
                    break;
                case "check":
                    CheckAccount(stream, dict);
                    break;
                case "search":
                    SearchUser(stream, dict);
                    break;
                case "others":
                    ReverseBuddies(stream, dict);
                    break;
                case "otherslist":
                    OnOthersList(stream, dict);
                    break;
                case "uniquesearch":
                    SuggestUniqueNickname(stream, dict);
                    break;
                case "profilelist":
                    OnProfileList(stream, dict);
                    break;
                case "pmatch":
                    MatchProduct(stream, dict);
                    break;
                case "newuser":
                    CreateUser(stream, dict);
                    break;
                default:
                    LogWriter.Log.Write("Received unknown request " + recieved[0], LogLevel.Debug);
                    GamespyUtils.SendGPError(stream, 0, "An invalid request was sended.");
                    stream.Close(); // This function force the client to disconnect

                    /*
                     * Since the client is already disconnected, we force the Server to free
                     * the allocated memory of the socket so we can save up memory space.
                     * 
                     * The server also does this at the shutdown, but keeping it in the
                     * shutdown process will mean having a lot of unused streams.
                     */
                    Release(stream);
                    break;
            }
        }

        private void SuggestUniqueNickname(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("uniquesearch", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
            stream.Close();
            Release(stream);
        }

        private void OnProfileList(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("profilelist", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
            stream.Close();
            Release(stream);
        }

        private void MatchProduct(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("pmatch", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
            stream.Close();
            Release(stream);
        }

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void CreateUser(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("newuser", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
            stream.Close();
            Release(stream);
        }

        private void OnOthersList(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("otherslist", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
            stream.Close();
            Release(stream);
        }

        private void ReverseBuddies(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            //            PrintReceivedDictToLogger("others", dict);
            //            SendErrorAndFreeStream(stream, 0, "This request is not supported yet.");

            // TODO: Please finis this function
            stream.SendAsync(@"\others\\odone\final\");
        }

        private void SearchUser(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("search", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
            stream.Close();
            Release(stream);
        }

        private void CheckAccount(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("check", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
            stream.Close();
            Release(stream);
        }

        private void RetriveNicknames(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            string password = "";
            bool sendUniqueNick = false;

            if (!dict.ContainsKey("email"))
            {
                GamespyUtils.SendGPError(stream, 1, "There was an error parsing an incoming request.");
                stream.Close();
                Release(stream);
                return;
            }
            
            // First, we try to receive an encoded password
            if(!dict.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!dict.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue
                    GamespyUtils.SendGPError(stream, 1, "There was an error parsing an incoming request.");
                    stream.Close();
                    Release(stream);
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

            List<Dictionary<string, object>> queryResult = null;

            try
            {
                queryResult = databaseDriver.Query("SELECT profiles.nick, profiles.uniquenick FROM profiles INNER JOIN users ON profiles.userid=users.userid WHERE LOWER(users.email)=@P0 AND LOWER(users.password)=@P1", dict["email"].ToLowerInvariant(), password.ToLowerInvariant());
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Error);
                GamespyUtils.SendGPError(stream, 4, "This request cannot be processed because of a database error.");
                stream.Close();
                Release(stream);
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
            password = @"\nr\";

            foreach (Dictionary<string, object> row in queryResult)
            {
                password += @"\nick\";
                password += row["nick"];

                if (sendUniqueNick)
                {
                    password += @"\uniquenick\";
                    password += row["uniquenick"];
                }
            }

            password += @"\ndone\final\";
            stream.SendAsync(password);

            /* Legacy C++ code to reimpliment 
            bool PSServer::OnSendNicks(mdk_socket stream, const char *buf, int)
            {
                std::string email = "", pass = "", gamename = "", str = "";
                bool bSendUnique = false;
                size_t i = 0;
                CResultSet *result = NULL;

                // Get data from buffer

                if (!get_gs_data(buf, email, "email"))
                    return false;

                if (get_gs_data(buf, pass, "passenc"))
                {
                    // Uncrypt the password
                    gs_pass_decode(pass);
                }
                else
                {
                    if (!get_gs_data(buf, pass, "pass"))
                        return false;
                }

                if (get_gs_data(buf, gamename, "gamename"))
                    bSendUnique = true;

                // Create the query and execute it
                str = "SELECT profiles.nick, profiles.uniquenick FROM profiles INNER "
                    "JOIN users ON profiles.userid=users.userid WHERE users.email='";
                if (!mdk_escape_query_string(m_lpDatabase, email))
                    return false;

                str += email;
                str += "' AND password='";
                if (!mdk_escape_query_string(m_lpDatabase, pass))
                    return false;

                str += pass;
                str += "'";

                result = new CResultSet();

                if (!result->ExecuteQuery(m_lpDatabase, str))
                {
                    delete result;

                    WriteTCP(stream, "\\nr\\\\ndone\\final\\");
                    return false;
                }

                if (!result->GotoFirstRow())
                {
                    delete result;

                    WriteTCP(stream, "\\nr\\\\ndone\\final\\");
                    return false;

                }

                str = "\\nr\\" + std::to_string(result->GetTotalRows());

                // Get all the nicks and store them
                do
                {
                    str += "\\nick\\";
                    str += result->GetStringFromRow(0);

                    if (bSendUnique)
                    {
                        str += "\\uniquenick\\";
                        str += result->GetStringFromRow(1);
                    }
                } while(result->GotoNextRow());

                str += "\\ndone\\final\\";

                // Send to the socket
                WriteTCP(stream, str);

                delete result;

                return true;
            }*/

        }

        /// <summary>
        /// Checks if a provided email is valid
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void IsEmailValid(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {
                GamespyUtils.SendGPError(stream, 1, "There was an error parsing an incoming request.");
                stream.Close();
                Release(stream);
                return;
            }

            try
            {
                if (databaseDriver.Query("SELECT userid FROM users WHERE LOWER(email)=@P0", dict["email"].ToLowerInvariant()).Count != 0)
                    stream.SendAsync(@"\vr\1\final\");
                else
                    stream.SendAsync(@"\vr\0\final\");
            }
            catch (Exception ex)
            {
                LogWriter.Log.WriteException(ex);
                GamespyUtils.SendGPError(stream, 4, "This request cannot be processed because of a database error.");
                stream.Close();
                Release(stream);
            }
        }
    }
}
