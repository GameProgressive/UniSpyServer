using GameSpyLib.Database;
using GameSpyLib.Network;
using QueryReport.Structure;
using QueryReport.Handler.ServerList;
using System.Collections.Concurrent;
using System.Net;
using System.Timers;
using QueryReport.Handler;

namespace QueryReport.Server
{
    public class QRServer : TemplateUdpServer
    {

        /// <summary>
        /// A List of all servers that have sent data to this master server, and are active in the last 30 seconds or so
        /// </summary>
        public static ConcurrentDictionary<string, GameServerData> ServersList = new ConcurrentDictionary<string, GameServerData>();

        /// <summary>
        /// A timer that is used to Poll all the servers, and remove inactive servers from the server list
        /// </summary>
        private static Timer PollTimer;

        /// <summary>
        /// The Time for servers are to remain in the serverlist since the last ping.
        /// Once this value is surpassed, server is presumed offline and is removed
        /// </summary>
        public static int ServerTTL { get; protected set; } = 30;

        public bool IsChallengeSent = false;

        public bool HasInstantKey = false;


        public static DatabaseDriver DB;
        public QRServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {
            DB = databaseDriver;
            //The Time for servers to remain in the serverlist since the last ping in seconds.
            //This value must be greater than 20 seconds, as that is the ping rate of the server
            //Suggested value is 30 seconds, this gives the server some time if the master server
            //is busy and cant refresh the server's TTL right away


            // Setup timer. Remove servers who havent ping'd since ServerTTL
            PollTimer = new Timer(5000);
            PollTimer.Elapsed += (s, e) => ServerListHandler.CheckServers();
            PollTimer.Start();
        }

        protected override void OnReceived(EndPoint endPoint, byte[] message)
        {
            CommandSwitcher.Switch(this, endPoint, message);
        }

        private bool _disposed;
        protected override void Dispose(bool disposingManageResource)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposingManageResource)
            { }
            DB?.Close();
            DB?.Dispose();
        }
    }
}
