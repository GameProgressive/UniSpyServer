using System.Net;
using NetCoreServer;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Http.Server
{
    public class HttpConnectionManager : NetCoreServer.HttpServer, IConnectionManager
    {
        public event OnConnectingEventHandler OnInitialization;
        public HttpConnectionManager(IPEndPoint endpoint) : base(endpoint)
        {
        }

        protected override NetCoreServer.TcpSession CreateSession() => new UniSpy.Server.Core.Network.Http.Server.HttpConnection(this);
        protected override void OnConnecting(TcpSession connection)
        {
            OnInitialization((HttpConnection)connection);
            base.OnConnecting(connection);
        }

        public new void Start() => base.Start();

    }
}