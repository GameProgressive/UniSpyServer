using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Network.Tcp.Server;
using UniSpy.Server.Core.Abstraction.Interface;
using System.Net;

namespace UniSpy.Server.Chat.Application
{
    public sealed class Server : ServerBase
    {
        static Server()
        {
            _name = "Chat";
        }
        public Server() { }

        public Server(IConnectionManager manager) : base(manager) { }

        public override void Start()
        {
            base.Start();
            _ = Chat.Application.StorageOperation.Persistance.PeerGroupList;
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(endPoint);
    }
}