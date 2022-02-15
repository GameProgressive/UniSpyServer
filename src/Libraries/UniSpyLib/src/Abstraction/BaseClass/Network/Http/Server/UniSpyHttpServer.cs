using System;
using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class UniSpyHttpServer : HttpServer, IServer
    {
        public Guid ServerID { get; private set; }
        protected UniSpyHttpServer(Guid serverID, IPEndPoint endpoint) : base(endpoint)
        {
        }
    }
}