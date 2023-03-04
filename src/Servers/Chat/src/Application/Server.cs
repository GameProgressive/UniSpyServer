using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.Chat.Application
{
    internal sealed class Server : UniSpy.Server.Core.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public static GeneralMessageChannel GeneralChannel = new GeneralMessageChannel();
        public Server(UniSpyServerConfig config) : base(config)
        {
        }
        public override void Start()
        {
            base.Start();
            GeneralChannel.StartSubscribe();
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}