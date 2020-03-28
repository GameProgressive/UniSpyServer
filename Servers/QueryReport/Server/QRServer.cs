using GameSpyLib.Database.Entity;
using GameSpyLib.Network;
using QueryReport.Handler.CommandHandler.ServerList;
using QueryReport.Handler.CommandSwitcher;
using QueryReport.Handler.SystemHandler.PeerSystem;
using System.Net;

namespace QueryReport.Server
{
    public class QRServer : TemplateUdpServer
    {
        /// <summary>
        /// A List of all servers that have sent data to this master server, and are active in the last 30 seconds or so
        /// </summary>
        //public static ConcurrentDictionary<EndPoint, DedicatedGameServer> GameServerList = new ConcurrentDictionary<EndPoint, DedicatedGameServer>();

        public bool IsChallengeSent = false;

        public bool HasInstantKey = false;

        private ServerListChecker _checker = new ServerListChecker();

        public QRServer(string serverName, DatabaseEngine engine, IPAddress address, int port) : base(serverName, address, port)
        {
            new PeerGroupHandler().LoadAllGameGroups();
            //The Time for servers to remain in the serverlist since the last ping in seconds.
            //This value must be greater than 20 seconds, as that is the ping rate of the server
            //Suggested value is 30 seconds, this gives the server some time if the master server
            //is busy and cant refresh the server's TTL right away
            _checker.StartCheck(this);
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            CommandSwitcher.Switch(this, endPoint, message);
        }

        private bool _disposed;

        protected override void Dispose(bool disposingManageResource)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposingManageResource)
            {
            }
        }
    }
}
