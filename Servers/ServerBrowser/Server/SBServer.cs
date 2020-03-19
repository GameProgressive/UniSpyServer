using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace ServerBrowser
{
    /// <summary>
    /// This class emulates the master.gamespy.com TCP server on port 28910.
    /// This server is responisible for sending server lists to the online server browser in the game.
    /// </summary>
    public class SBServer : TemplateTcpServer
    {
        public static DatabaseEngine DB;

        public SBServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = engine;
        }

        protected override TcpSession CreateSession() { return new SBSession(this); }

        private bool _disposed;

        protected override void Dispose(bool disposingManagedResources)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposingManagedResources)
            {
            }

            base.Dispose(disposingManagedResources);
        }
    }
}
