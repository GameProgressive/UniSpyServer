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
        public RedisChannelSubscriber ChannelSubScriber { get; private set; }
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
            ChannelSubScriber = new RedisChannelSubscriber();
        }

        public override bool Start()
        {
            ChannelSubScriber.StartSubscribe();
            return base.Start();
        }

        protected override UniSpyUdpSession CreateSession(EndPoint endPoint) => new Session(this, endPoint);


    }
}
