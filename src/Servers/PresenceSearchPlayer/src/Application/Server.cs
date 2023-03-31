using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Network.Tcp.Server;

namespace UniSpy.Server.PresenceSearchPlayer.Application
{
    public sealed class Server : ServerBase
    {
        static Server()
        {
            _name = "PresenceSearchPlayer";
        }
        public Server()
        {
        }

        public Server(IConnectionManager manager) : base(manager)
        {
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(endPoint);
    }
}