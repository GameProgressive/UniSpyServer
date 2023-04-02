using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Network.Http.Server;

namespace UniSpy.Server.WebServer.Application
{
    public sealed class Server : ServerBase
    {
        static Server()
        {
            _name = "WebServer";
        }
        public Server(){ }

        public Server(IConnectionManager manager) : base(manager){}

        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new HttpConnectionManager(endPoint);
    }
}