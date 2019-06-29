using System;
using System.Collections.Generic;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
using RetroSpyServer.Application;
using RetroSpyServer.DBQueries;

namespace RetroSpyServer.Servers.GPSP
{
    public class GPSPClient : IDisposable
    {
        /// <summary>
        /// A unqie identifier for this connection
        /// </summary>
        public long ConnectionID;

        /// <summary>
        /// Indicates whether this object is disposed
        /// </summary>
        public bool Disposed { get; protected set; } = false;

        /// <summary>
        /// The clients socket network stream
        /// </summary>
        public TcpStream Stream { get; set; }

        /// <summary>
        /// Event fired when the connection is closed
        /// </summary>
        public static event GpspConnectionClosed OnDisconnect;

        // private DatabaseDriver databaseDriver;
        private GPSPDBQuery DBQuery;

        string sendingBuffer;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public GPSPClient(TcpStream stream, long connectionId, DatabaseDriver dbdriver)
        {
            //databaseDriver = driver;
            //pass the dbdriver to GPSPDBQuery let GPSPDBQuery class hanle for us
            DBQuery = new GPSPDBQuery(dbdriver);

            // Generate a unique name for this connection
            ConnectionID = connectionId;

            // Init a new client stream class
            Stream = stream;
            Stream.OnDisconnect += () => Dispose();

            Stream.IsMessageFinished += (string message) =>
            {
                if (message.EndsWith("\\final\\"))
                    return true;

                return false;
            };

            Stream.DataReceived += (message) =>
            {
                // Read client message, and parse it into key value pairs
                ProcessDataReceived(message);
            };
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GPSPClient()
        {
            if (!Disposed)
                Dispose();
        }

        public void Dispose()
        {
            // Only dispose once
            if (Disposed) return;
            Dispose(false);
        }

        /// <summary>
        /// Dispose method to be called by the server
        /// </summary>
        public void Dispose(bool DisposeEventArgs = false)
        {
            // Only dispose once
            if (Disposed) return;
            Disposed = true;

            // If connection is still alive, disconnect user
            if (!Stream.SocketClosed)
                Stream.Close(DisposeEventArgs);

            // Call disconnect event
            OnDisconnect?.Invoke(this);
        }

        /// <summary>
        /// This is the primary method for fetching an accounts BF2 PID
        /// </summary>
        /// <param name="recvData"></param>
        /*private void SendCheck(Dictionary<string, string> recvData)
        {
            // Make sure we have the needed data
            if (!recvData.ContainsKey("nick"))
            {
                handler.SendAsync(@"\error\\err\0\fatal\\errmsg\Invalid Query!\id\1\final\");
                return;
            }

            // Try to get user data from database
            try
            {
                using (GamespyDatabase Db = new GamespyDatabase())
                {
                    int pid = Db.GetPlayerId(recvData["nick"]);
                    if (pid == 0)
                        handler.SendAsync(@"\error\\err\265\fatal\\errmsg\Username [{0}] doesn't exist!\id\1\final\", recvData["nick"]);
                    else
                        handler.SendAsync(@"\cur\0\pid\{0}\final\", pid);
                }
            }
            catch
            {
                handler.SendAsync(@"\error\\err\265\fatal\\errmsg\Database service is Offline!\id\1\final\");
                //Dispose();
            }
        }*/


        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected void ProcessDataReceived(string message)
        {
            if (message[0] != '\\')
            {
                GamespyUtils.SendGPError(Stream, 0, "An invalid request was sended.");
                return;
            }

            string[] recieved = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = GamespyUtils.ConvertGPResponseToKeyValue(recieved);

            switch (recieved[0])
            {
                case "valid":
                    IsEmailValid(Stream, dict);
                    break;
                case "nicks":
                    RetriveNicknames(Stream, dict);
                    break;
                case "check":
                    CheckAccount(Stream, dict);
                    break;
                case "search":
                    SearchUser(Stream, dict);
                    break;
                case "others":
                    ReverseBuddies(Stream, dict);
                    break;
                case "otherslist":
                    OnOthersList(Stream, dict);
                    break;
                case "uniquesearch":
                    SuggestUniqueNickname(Stream, dict);
                    break;
                case "profilelist":
                    OnProfileList(Stream, dict);
                    break;
                case "pmatch":
                    MatchProduct(Stream, dict);
                    break;
                case "newuser":
                    CreateUser(Stream, dict);
                    break;
                default:
                    LogWriter.Log.Write("Received unknown request " + recieved[0], LogLevel.Debug);
                    GamespyUtils.SendGPError(Stream, 0, "An invalid request was sended.");
                    break;
            }
        }

        private void SuggestUniqueNickname(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("uniquesearch", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        private void OnProfileList(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("profilelist", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        private void MatchProduct(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("pmatch", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void CreateUser(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("newuser", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        private void OnOthersList(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("otherslist", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        private void ReverseBuddies(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("others", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");

            // TODO: Please finis this function
            //stream.SendAsync(@"\others\\odone\final\");
        }

        private void SearchUser(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("search", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        private void CheckAccount(TcpStream stream, Dictionary<string, string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("check", dict);
            GamespyUtils.SendGPError(stream, 0, "This request is not supported yet.");
        }

        /// <summary>
        /// This method is requested by the client when logging in to fetch all the account
        /// names that have the specified email address and password combination
        /// </summary>
        /// <param name="recvData"></param>
        private void RetriveNicknames(TcpStream stream, Dictionary<string, string> dict)
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

        /// <summary>
        /// Checks if a provided email is valid
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void IsEmailValid(TcpStream stream, Dictionary<string, string> dict)
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
                    stream.SendAsync(@"\vr\0\final\");
            }
            catch (Exception ex)
            {
                LogWriter.Log.WriteException(ex);
                GamespyUtils.SendGPError(stream, 4, "This request cannot be processed because of a database error.");
            }
        }

    }
}
