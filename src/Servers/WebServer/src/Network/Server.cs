using System;
using System.Net;
using NetCoreServer;
using UniSpyLib.Abstraction.BaseClass.Network.Http.Server;

namespace WebServer.Network
{
    internal sealed class Server : UniSpyHttpServer
    {
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
            SessionManager = new SessionManager();
        }
        protected override TcpSession CreateSession() => new Session(this);
    }
}