using System;
using System.Net;
using NetCoreServer;
using UniSpyLib.Abstraction.BaseClass.Network.Http.Server;

namespace WebServer.Network
{
    internal class Server : UniSpyHttpServer
    {
        public Server(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
        }
        protected override TcpSession CreateSession() => new Session(this);
    }
}