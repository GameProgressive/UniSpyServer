using GameSpyLib.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.Stats
{
    public class GStatsClient : IDisposable
    {
        /// <summary>
        /// A unqie identifier for this connection
        /// </summary>
        public long ConnectionID;

        /// <summary>
        /// The clients socket network stream
        /// </summary>
        public TcpStream Stream { get; set; }

        /// <summary>
        /// Indicates whether this object is disposed
        /// </summary>
        public bool Disposed { get; protected set; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public GStatsClient(TcpStream stream, long connectionId)
        {
            // Generate a unique name for this connection
            ConnectionID = connectionId;

            // Init a new client stream class
            Stream = stream;
            Stream.OnDisconnect += Dispose;
            //determine whether gamespy request is finished
            Stream.IsMessageFinished += Stream_IsMessageFinished;
            // Read client message, and parse it into key value pairs
            Stream.DataReceived += Stream_DataReceived;

        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GStatsClient()
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
                Stream.OnDisconnect -= Dispose;
                //determine whether gamespy request is finished
                Stream.IsMessageFinished -= Stream_IsMessageFinished;
                // Read client message, and parse it into key value pairs
                Stream.DataReceived -= Stream_DataReceived;
                // If connection is still alive, disconnect user
                if (!Stream.SocketClosed)
                    Stream.Close(DisposeEventArgs);
            }
            catch { }

            Disposed = true;
        }

        private bool Stream_IsMessageFinished(string message)
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
        protected void Stream_DataReceived(string message)
        {
            
        }
    }
}
