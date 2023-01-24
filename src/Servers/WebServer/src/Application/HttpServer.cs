using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Application
{
    internal sealed class HttpServer : UniSpyLib.Abstraction.BaseClass.Network.Http.Server.HttpServer
    {
        public HttpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(serverID, serverName, endpoint)
        {
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}