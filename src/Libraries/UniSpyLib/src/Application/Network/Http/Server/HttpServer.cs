using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Application.Network.Http.Server
{
    public class HttpServer : NetCoreServer.HttpServer, IServer
    {
        public Guid ServerID { get; private set; }
        public string ServerName { get; private set; }
        protected HttpServer(Guid serverID, string serverName, IPEndPoint endpoint) : base(endpoint)
        {
            ServerName = serverName;
        }
        protected override NetCoreServer.TcpSession CreateSession()
        {
            var session = new HttpSession(this);
            ClientBase.CreateClient(session);
            return session;
        }
    }
}