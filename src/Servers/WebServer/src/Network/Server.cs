using System;
using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server;

namespace UniSpyServer.Servers.WebServer.Network
{
    public sealed class Server : UniSpyHttpServer
    {
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
        }
        protected override TcpSession CreateSession() => new Session(this);
    }
}