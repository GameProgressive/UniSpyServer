using GameSpyLib.Database;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace StatsAndTracking
{
    public class GStatsServer : TemplateTcpServer
    {
        public static DatabaseDriver DB;

        public bool Disposed = false;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// If the databaseDriver is null, then the server will attempt to create it's own connection
        /// otherwise it will use the specified connection
        /// </param>
        public GStatsServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {
            //GStatsHandler.DBQuery = new GSTATSDBQuery(databaseDriver);
            DB = databaseDriver;
        }
        protected override TcpSession CreateSession() { return new GstatsSession(this); }

        protected override void Dispose(bool disposingManagedResources)
        {
            if (disposingManagedResources)
            {

            }
            DB.Close();
            DB?.Dispose();
            DB = null;

            base.Dispose(disposingManagedResources);
        }
    }
}
