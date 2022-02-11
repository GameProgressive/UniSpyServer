using System;
using System.Net;
using UniSpyServer.Servers.QueryReport.Handler.SystemHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;
using UniSpyServer.UniSpyLib.Abstraction.Contract;

namespace UniSpyServer.Servers.QueryReport.Network
{
    [ServerName("QueryReport")]
    public sealed class Server : UniSpyUdpServer
    {
        /// <summary>
        /// We use this redis channel to exchange data between servers
        /// </summary>
        /// <value></value>
        public RedisChannel InfoExchangeChannel { get; private set; }
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
            InfoExchangeChannel = new RedisChannel();
        }

        public override bool Start()
        {
            InfoExchangeChannel.StartSubscribe();
            return base.Start();
        }

        protected override UniSpyUdpSession CreateSession(EndPoint endPoint) => new Session(this, endPoint);


    }
}
