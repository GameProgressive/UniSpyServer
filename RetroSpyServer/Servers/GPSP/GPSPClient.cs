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

        private GPSPHelper Helper;
       
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public GPSPClient(TcpStream stream, long connectionId, DatabaseDriver dbdriver)
        {
            //databaseDriver = driver;
            Helper = new GPSPHelper(dbdriver);
            //pass the dbdriver to GPSPDBQuery let GPSPDBQuery class hanle for us            

            // Generate a unique name for this connection
            ConnectionID = connectionId;

            // Init a new client stream class
            Stream = stream;
            Stream.OnDisconnect += () => Dispose();
            //determine whether gamespy request is finished
            Stream.IsMessageFinished += (string message) =>
            {
                if (message.EndsWith("\\final\\"))
                    return true;

                return false;
            };
            // Read client message, and parse it into key value pairs
            Stream.DataReceived += (message) =>ProcessDataReceived(message);
            
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

            //split the command to key value pairs
            string[] recieved = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = GamespyUtils.ConvertGPResponseToKeyValue(recieved);
            //determin which command client is requested
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
            Helper.SuggestUniqueNickname(stream, dict);
        }

        private void OnProfileList(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.OnProfileList(stream, dict);
        }

        private void MatchProduct(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.MatchProduct(stream, dict);
        }

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void CreateUser(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.CreateUser(stream, dict);
        }

        private void OnOthersList(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.OnOthersList(stream, dict);

        }

        private void ReverseBuddies(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.ReverseBuddies(stream, dict);


            // TODO: Please finis this function
            //stream.SendAsync(@"\others\\odone\final\");
        }

        private void SearchUser(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.SearchUser(stream, dict);
        }

        private void CheckAccount(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.CheckAccount(stream, dict);

        }

        /// <summary>
        /// This method is requested by the client when logging in to fetch all the account
        /// names that have the specified email address and password combination
        /// </summary>
        /// <param name="recvData"></param>
        private void RetriveNicknames(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.RetriveNicknames(stream, dict);
        }

        /// <summary>
        /// Checks if a provided email is valid
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void IsEmailValid(TcpStream stream, Dictionary<string, string> dict)
        {
            Helper.IsEmailValid(stream, dict);
        }
    }
}
