using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Network.Tcp.Server;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Aggregate.Redis;
using UniSpy.Server.PresenceConnectionManager.Abstraction.Interface;

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
        protected override IClient HandleConnectionInitialization(IConnection connection)
        {
            var client = (IShareClient)base.HandleConnectionInitialization(connection);
            if (client.Info.IsRemoteClient)
            {
                var info = client.Info;
                info.IsRemoteClient = false;
                // we parse the info into our client
                client = new Client(connection, this, info);
            }
            return client;
        }
        public override void Start()
        {
            base.Start();
            UserInfoChannel.Subscribe();
        }
    }
}