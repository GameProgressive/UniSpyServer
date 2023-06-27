using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Network.Tcp.Server;
using UniSpy.Server.Core.Abstraction.Interface;
using System.Net;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Application
{
    public sealed class Server : ServerBase
    {
        public static readonly GeneralMessageChannel GeneralChannel = new GeneralMessageChannel();
        static Server()
        {
            _name = "Chat";
        }
        public Server() { }

        public Server(IConnectionManager manager) : base(manager) { }

        public override void Start()
        {
            base.Start();
            GeneralChannel.Subscribe();
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new TcpConnectionManager(endPoint);
        protected override IClient HandleConnectionInitialization(IConnection connection)
        {
            var client = (IShareClient)base.HandleConnectionInitialization(connection);
            if (client.Info.IsRemoteClient)
            {
                var info = client.Info;
                info.IsRemoteClient = false;
                // we parse info to our client
                client = new Client(connection, this, info);

            }
            return client;
        }
    }
}