using System;
using System.Net;
using NetCoreServer;
using UniSpyServer.Servers.QueryReport.Handler.SystemHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
using UniSpyServer.UniSpyLib.Abstraction.Contract;

namespace UniSpyServer.Servers.ServerBrowser.Network
{
    /// <summary>
    /// This class emulates the master.gamespy.com TCP server on port 28910.
    /// This server is responisible for sending server lists to the online server browser in the game.
    /// </summary>
    [ServerName("ServerBrowser")]
    public sealed class Server : UniSpyTcpServer
    {
        public RedisChannel InfoExchangeChannel { get; private set; }
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
            InfoExchangeChannel = new RedisChannel();
        }
        protected override TcpSession CreateSession() => new Session(this);
    }
}
