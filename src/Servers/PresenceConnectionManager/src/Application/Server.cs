using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Network.Tcp.Server;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    public sealed class Server : ServerBase
    {
        static Server()
        {
            _name = "PresenceConnectionManager";
        }
        public Server() { }
        public Server(IConnectionManager manager) : base(manager) { }
        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);
        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(IPEndPoint.Parse("0.0.0.0:29900"));
    }
}