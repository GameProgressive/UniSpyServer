using System.Linq;
using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Network.Tcp.Server;

namespace UniSpy.Server.ServerBrowser.V1.Application
{
    public sealed class Server : ServerBase
    {
        static Server()
        {
            _name = "ServerBrowserV1";
        }
        public Server()
        {
        }

        public Server(IConnectionManager manager) : base(manager)
        {
        }
        // protected override IClient CreateClient(IConnection connection) => new Client(connection, this);
        protected override IClient CreateClient(IConnection connection) => throw new System.NotImplementedException("implement v1 client");


        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(endPoint);
    }
}