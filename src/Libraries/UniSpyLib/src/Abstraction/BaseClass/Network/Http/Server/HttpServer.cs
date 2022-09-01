using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class HttpServer : NetCoreServer.HttpServer, IServer
    {
        public Guid ServerID { get; private set; }
        public string ServerName { get; private set; }
        IPEndPoint IServer.Endpoint => (IPEndPoint)Endpoint;
        public HttpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(endpoint)
        {
            ServerName = serverName;
        }
        protected abstract IClient CreateClient(ISession session);
        protected override NetCoreServer.TcpSession CreateSession()
        {
            var session = new HttpSession(this);
            CreateClient(session);
            return session;
        }
    }
}