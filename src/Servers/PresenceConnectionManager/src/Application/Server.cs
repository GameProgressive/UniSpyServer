using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    internal sealed class Server : UniSpy.Server.Core.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public Server(UniSpyServerConfig config) : base(config)
        {
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}