using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Network.Tcp.Server;

namespace UniSpy.Server.Master.Application
{
    /// <summary>
    /// Master server is used for old games, and it function like the combination of QueryReport and ServerBrowser
    /// game server reports its data to Master server and game client retrive server list from Master server
    /// </summary>
    public sealed class Server : ServerBase
    {
        static Server()
        {
            _name = "Master";
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