using NetCoreServer;
using QueryReport.Handler.SystemHandler;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;

namespace ServerBrowser.Network
{
    /// <summary>
    /// This class emulates the master.gamespy.com TCP server on port 28910.
    /// This server is responisible for sending server lists to the online server browser in the game.
    /// </summary>
    internal sealed class Server : UniSpyTcpServer
    {
        public RedisChannelSubscriber RedisChannelSubscriber { get; private set; }
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SBSessionManager();
            RedisChannelSubscriber = new RedisChannelSubscriber();
        }
        protected override TcpSession CreateSession() => new Session(this);
    }
}
