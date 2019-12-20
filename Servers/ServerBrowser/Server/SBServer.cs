using GameSpyLib.Database;
using GameSpyLib.Network;
using System.Net;

namespace ServerBrowser
{
    /// <summary>
    /// This class emulates the master.gamespy.com TCP server on port 28910.
    /// This server is responisible for sending server lists to the online server browser in the game.
    /// </summary>
    public class SBServer : TemplateTcpServer
    {

        public static DatabaseDriver DB;
        public SBServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = databaseDriver;
        }

        private bool _disposed;
        protected override void Dispose(bool disposingManagedResources)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposingManagedResources)
            {

            }
            DB.Close();
            DB.Dispose();
            base.Dispose(disposingManagedResources);
        }
    }
}
