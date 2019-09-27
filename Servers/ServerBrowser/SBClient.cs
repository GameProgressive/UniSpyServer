using GameSpyLib.Network;
using QueryReport;
using QueryReport.GameServerInfo;
using System;
using System.Linq;

namespace ServerBrowser
{
    public class SBClient
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
        public TCPStream Stream { get; protected set; }

        /// <summary>
        /// Event fired when the connection is closed
        /// </summary>
        public static event ServerBrowserConnectionClosed OnDisconnect;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public SBClient(TCPStream stream, long connectionId)
        {
            // Generate a unique name for this connection
            ConnectionID = connectionId;
            // Init a new client stream class
            Stream = stream;
            Stream.OnDisconnected += Dispose;
            Stream.OnDataReceived += ProcessData;
        }

        protected void ProcessData(string received)
        {
            // lets split up the message based on the delimiter
            string[] messages = received.Split(new string[] { "\x00\x00\x00\x00" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string message in messages)
            {
                // Ignore Non-BF2 related queries
                if (message.StartsWith("battlefield2"))
                    SBHandler.ParseRequest(this, message);
            }
            //we get the server list from query report server
            IQueryable<GameServer> servers = 
                QRServer.Servers.ToList().Select(x => x.Value).Where(x => x.IsValidated).AsQueryable();




        }


        /// <summary>
        /// Destructor
        /// </summary>
        ~SBClient()
        {
            if (!Disposed)
                Dispose(false);
        }

        public void Dispose()
        {
            if (!Disposed)
                Dispose(false);
        }

        /// <summary>
        /// Dispose method to be called by the server
        /// </summary>
        public void Dispose(bool DisposeEventArgs = false)
        {
            // Only dispose once
            if (Disposed) return;

            // Preapare to be unloaded from memory
            Disposed = true;

            // If connection is still alive, disconnect user
            if (!Stream.SocketClosed)
                Stream.Close(DisposeEventArgs);

            // Call disconnect event
            if (OnDisconnect != null)
                OnDisconnect(this);
        }


    }
}
