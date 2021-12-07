using NetCoreServer;
using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
using UniSpyServer.UniSpyLib.Abstraction.Contract;

//GPCM represents GameSpy Connection Manager
namespace UniSpyServer.Servers.PresenceConnectionManager.Network
{
    /// <summary>
    /// This server emulates the Gamespy Client Manager Server on port 29900.
    /// This class is responsible for managing the login process.
    /// </summary>
    [ServerName("PresenceConnectionManager")]
    public sealed class Server : UniSpyTcpServer
    {
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
        }

        protected override TcpSession CreateSession() => new Session(this);

    }
}