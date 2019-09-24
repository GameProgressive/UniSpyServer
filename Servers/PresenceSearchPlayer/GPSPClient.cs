using GameSpyLib.Common;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Handler;

namespace PresenceSearchPlayer
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
        public static event GPSPConnectionClosed OnDisconnected;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public GPSPClient(TcpStream stream, long connectionId)
        {
            // Generate a unique name for this connection
            ConnectionID = connectionId;

            // Init a new client stream class
            Stream = stream;

            //determine whether gamespy request is finished
            Stream.IsMessageFinished += IsMessageFinished;

            // Read client message, and parse it into key value pairs
            Stream.OnDataReceived += ProcessData;

            //Dispose when client disconnected
            Stream.OnDisconnected += Dispose;

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

            try
            {
                Stream.OnDisconnected -= Dispose;
                //determine whether gamespy request is finished
                Stream.IsMessageFinished -= IsMessageFinished;
                // Read client message, and parse it into key value pairs
                Stream.OnDataReceived -= ProcessData;
                // If connection is still alive, disconnect user
                if (!Stream.SocketClosed)
                    Stream.Close(DisposeEventArgs);
            }
            catch { }


            // Call disconnect event
            OnDisconnected?.Invoke(this);

            Disposed = true;
        }
        private bool IsMessageFinished(string message)
        {
            if (message.EndsWith("\\final\\"))
                return true;
            else
                return false;
        }
        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected void ProcessData(string message)
        {
            if (message[0] != '\\')
            {
                GameSpyUtils.SendGPError(Stream, GPErrorCode.Parse, "An invalid request was sended.");
                return;
            }

            string[] submessage = message.Split("\\final\\");

            foreach (string command in submessage)
            {
                if (command.Length < 1)
                    continue;

                // Read client message, and parse it into key value pairs
                string[] recieved = command.TrimStart('\\').Split('\\');
                Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);
                try
                {
                    switch (recieved[0])
                    {
                        case "search":
                            SearchHandler.SearchUsers(this, dict);
                            break;
                        case "valid":
                            ValidHandler.IsEmailValid(this, dict);
                            break;
                        case "nicks":
                            NickHandler.SearchNicks(this, dict);
                            break;
                        case "pmatch":
                            PmatchHandler.PlayerMatch(this, dict);
                            break;
                        case "check":
                            CheckHandler.CheckProfileId(this, dict);
                            break;
                        case "newuser":
                            NewUserHandler.NewUser(this, dict);
                            break;
                        case "searchunique":
                            SearchUniqueHandler.SearchProfileWithUniquenick(this, dict);
                            break;
                        case "others":
                            OthersHandler.SearchOtherBuddy(this, dict);
                            break;
                        case "otherslist":
                            OthersListHandler.SearchOtherBuddyList(this, dict);
                            break;
                        case "uniquesearch":
                            UniqueSearchHandler.SuggestUniqueNickname(this, dict);
                            break;
                        case "profilelist":
                            ProfileListHandler.OnProfileList(this, dict);
                            break;
                        default:
                            LogWriter.Log.Write("[GPSP] received unknown data " + recieved[0], LogLevel.Debug);
                            GameSpyUtils.SendGPError(Stream, GPErrorCode.Parse, "An invalid request was sended.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogWriter.Log.WriteException(e);
                }
            }
        }
    }
}
