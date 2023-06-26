using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Network.Tcp.Server;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Aggregate.Redis;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    public sealed class Server : ServerBase
    {
        public static readonly UserInfoChannel UserInfoChannel = new UserInfoChannel();
        static Server()
        {
            _name = "PresenceConnectionManager";
        }
        public Server() { }
        public Server(IConnectionManager manager) : base(manager) { }
        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);
        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(endPoint);
        public override void Start()
        {
            base.Start();
            UserInfoChannel.Subscribe();
        }
    }
}