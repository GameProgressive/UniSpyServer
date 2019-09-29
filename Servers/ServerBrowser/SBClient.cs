using GameSpyLib.Network;
using GameSpyLib.Network.TCP;
using QueryReport;
using QueryReport.GameServerInfo;
using System;
using System.Linq;

namespace ServerBrowser
{
    public class SBClient : TCPClientBase
    {
        /// <summary>
        /// Event fired when the connection is closed
        /// </summary>
        public static event ServerBrowserConnectionClosed OnDisconnect;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public SBClient(TCPStream stream, long connectionid) : base(stream, connectionid)
        {
            // Generate a unique name for this connection
            ConnectionID = connectionid;
            // Init a new client stream class
            Stream = stream;
            Stream.OnDisconnected += ClientDisconnected;
            Stream.OnDataReceived += ProcessData;
        }

        protected override void ProcessData(string message)
        {
            // lets split up the message based on the delimiter
            string[] messages = message.Split(new string[] { "\x00\x00\x00\x00" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in messages)
            {
                // Ignore Non-BF2 related queries
                if (command.StartsWith("battlefield2"))
                    SBHandler.ParseRequest(this, message);
            }
            //we get the server list from query report server
            IQueryable<GameServer> servers =
                QRServer.Servers.ToList().Select(x => x.Value).Where(x => x.IsValidated).AsQueryable();
        }


        /// <summary>
        ///
        /// <summary>
        /// Dispose method to be called by the server
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            // Only dispose once
            if (Disposed) return;

            // Preapare to be unloaded from memory


            // If connection is still alive, disconnect user
            if (!Stream.SocketClosed)
                Stream.Close(true);

            // Call disconnect event
            if (OnDisconnect != null)
                OnDisconnect(this);

            Disposed = true;
        }

        public override void Send(string sendingBuffer)
        {
            Stream.SendAsync(sendingBuffer);
        }

        public override void SendServerChallenge(uint serverID)
        {
            throw new NotImplementedException();
        }

        protected override void ClientDisconnected()
        {
            Dispose(true);
        }
    }
}
