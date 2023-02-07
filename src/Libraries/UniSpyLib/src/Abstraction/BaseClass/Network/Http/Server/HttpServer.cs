using System;
using System.Net;
using NetCoreServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Http.Server
{
    public abstract class HttpServer : NetCoreServer.HttpServer, IServer
    {
        public Guid ServerID { get; private set; }
        public string ServerName { get; private set; }
        IPEndPoint IServer.ListeningIPEndPoint => (IPEndPoint)Endpoint;
        public IPEndPoint PublicIPEndPoint { get; private set; }
        public HttpServer(UniSpyServerConfig config) : base(config.ListeningIPEndPoint)
        {
            ServerID = config.ServerID;
            ServerName = config.ServerName;
            PublicIPEndPoint = config.PublicIPEndPoint;
        }
        protected abstract IClient CreateClient(IConnection connection);
        protected override NetCoreServer.TcpSession CreateSession()
        {
            var connection = new UniSpyLib.Abstraction.BaseClass.Network.Http.Server.HttpConnection(this);
            return connection;
        }
        protected override void OnConnecting(TcpSession connection)
        {
            base.OnConnecting(connection);
            CreateClient((HttpConnection)connection);
        }

        public new void Start() => base.Start();

    }
}