using System;
using System.Net;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Application
{
    class HttpServer : UniSpyLib.Abstraction.BaseClass.Network.Http.Server.HttpServer
    {
        public HttpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(serverID, serverName, endpoint)
        {
        }

        public override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}