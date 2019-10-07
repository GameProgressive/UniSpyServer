using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using System;
using System.Net;
using System.Net.Sockets;

namespace PresenceSearchPlayer
{
    public class GPSPServer : TcpServer
    {
        /// <summary>
        /// Database connection
        /// </summary>
        public static DatabaseDriver DB;

        public bool Disposed = false;
        public GPSPServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = databaseDriver;
            Start();
        }
        protected override TcpSession CreateSession() { return new GPSPSession(this); }

        protected override void OnError(SocketError error)
        {
            string errorMsg = Enum.GetName(typeof(SocketError), error);
            LogWriter.Log.Write(errorMsg, LogLevel.Error);
        }
        protected override void Dispose(bool disposingManagedResources)
        {
            if (disposingManagedResources)
            {
                
            }
            DB?.Dispose();
            DB = null;

            base.Dispose(disposingManagedResources);
        }
    }
}
