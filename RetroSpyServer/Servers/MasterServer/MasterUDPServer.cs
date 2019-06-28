using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Network;
using System.Net;
using GameSpyLib.Logging;

namespace RetroSpyServer.Servers.MasterServer
{
    public class MasterUDPServer : GameSpyUDPConnector
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


        public MasterUDPServer(IPEndPoint bindTo, LogWriter DebugLog) : base(bindTo, MaxConnections)
        {
        }
        /// <summary>
        /// Callback method for when the UDP Master socket recieves a connection
        /// </summary>
        protected override void ProcessAccept(GameSpyUDPHandler handler)
        {

        }


        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);
    }
}
