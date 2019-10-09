using GameSpyLib.Database;
using GameSpyLib.Logging;
using QueryReport.DedicatedServerData;
using QueryReport.Handler;
using QueryReport.Structure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Timers;
using GameSpyLib.Network;
using System.Net.Sockets;
using System.Text;

namespace QueryReport
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
            PollTimer.Elapsed += (s, e) => RefreshServerList.CheckServers();
            PollTimer.Start();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            // Need at least 5 bytes
            if (size < 5 && size > 2048)
            {
                return;
            }
            //string message = Encoding.ASCII.GetString(buffer, 0, (int)size);
            base.OnReceived(endpoint, buffer, offset, size); // This logs the data we received

            byte[] message = new byte[(int)size];
            Array.Copy(buffer, 0, message, 0, (int)size);
            
            CommandSwitcher.Switch(this, message);
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
