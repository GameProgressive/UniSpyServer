using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Http.Server;

namespace WebServer.Network
{
    internal class WebServer : UniSpyHttpServer
    {
        public WebServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
        }
    }
}