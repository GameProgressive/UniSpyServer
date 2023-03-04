using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.NatNegotiation.Application
{
    internal sealed class Server : UniSpy.Server.Core.Abstraction.BaseClass.Network.Udp.Server.UdpServer
    {
        public Server(UniSpyServerConfig config) : base(config)
        {
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}