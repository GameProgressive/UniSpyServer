using System;
using System.Net;
using System.Collections.Concurrent;
using GameSpyLib.Network;
using GameSpyLib.Logging;
using RetroSpyServer.Servers.MasterServer.GameServerInfo;

namespace RetroSpyServer.Servers.MasterServer
{
    public class MasterUdpServer:UdpServer
    {
        /// <summary>
        /// Max number of concurrent open and active connections.
        /// </summary>
        /// <remarks>
        ///   While fast, the BF2Available requests will shoot out 6-8 times
        ///   per client while starting up BF2, so i set this alittle higher then usual.
        ///   Servers also post their data here, and a lot of servers will keep the
        ///   connections rather high.
        /// </remarks>
        public const int MaxConnections = 256;
        /// <summary>
        /// A List of all servers that have sent data to this master server, and are active in the last 30 seconds or so
        /// </summary>
        public static ConcurrentDictionary<string, GameServer> Servers = new ConcurrentDictionary<string, GameServer>();

        public MasterUdpServer(IPEndPoint bindTo, int MaxConnection) : base(bindTo, MaxConnections)
        {
        }
        /// <summary>
        /// Callback method for when the UDP Master socket recieves a connection
        /// </summary>
        protected override void ProcessAccept(UdpPacket packet)
        {

        }


        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

    }
}
