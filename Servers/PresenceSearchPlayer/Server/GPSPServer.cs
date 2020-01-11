using GameSpyLib.Database.DatabaseConnectionKeeper;
using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;
using System.Timers;

namespace PresenceSearchPlayer
{
    public class GPSPServer : TemplateTcpServer
    {
        /// <summary>
        /// Database connection
        /// </summary>
        public static DatabaseDriver DB;
        private static DBKeeper _dbKeeper;
       
        private bool _disposed;
        public GPSPServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = databaseDriver;
            _dbKeeper = new DBKeeper(databaseDriver);
            _dbKeeper.Run();
        }
        protected override TcpSession CreateSession() { return new GPSPSession(this); }


        protected override void Dispose(bool disposingManagedResources)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposingManagedResources)
            {

            }
            DB?.Dispose();
            DB = null;
            base.Dispose(disposingManagedResources);
        }

       
    }
}
