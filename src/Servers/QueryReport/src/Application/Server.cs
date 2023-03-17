using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.QueryReport.Application
{
    internal sealed class Server : UniSpy.Server.Core.Abstraction.BaseClass.Network.Udp.Server.UdpServer
    {
        public Server(UniSpyServerConfig config) : base(config)
        {
        }
        public override void Start()
        {
            StorageOperation.NatNegChannel.StartSubscribe();
            base.Start();
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}