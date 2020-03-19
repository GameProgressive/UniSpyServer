using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace StatsAndTracking
{
    public class GStatsServer : TemplateTcpServer
    {
        public static DatabaseEngine DB;

        public bool Disposed = false;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// If the databaseDriver is null, then the server will attempt to create it's own connection
        /// otherwise it will use the specified connection
        /// </param>
        public GStatsServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            //GStatsHandler.DBQuery = new GSTATSDBQuery(databaseDriver);
            DB = engine;
        }

        protected override TcpSession CreateSession() { return new GStatsSession(this); }

        protected override void Dispose(bool disposingManagedResources)
        {
            if (disposingManagedResources)
            {
            }

            base.Dispose(disposingManagedResources);
        }
    }
}
