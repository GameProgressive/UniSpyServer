using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace PresenceSearchPlayer
{
    public class GPSPServer : TemplateTcpServer
    {
        /// <summary>
        /// Database connection
        /// </summary>
        public static DatabaseEngine DBType { get; protected set; }
        private bool _disposed;
        public GPSPServer(string serverName, DatabaseEngine databaseType, IPAddress address, int port) : base(serverName, address, port)
        {
            DBType = databaseType;
        }
        protected override TcpSession CreateSession() { return new GPSPSession(this); }


        protected override void Dispose(bool disposingManagedResources)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposingManagedResources)
            {

            }
            base.Dispose(disposingManagedResources);
        }


    }
}
