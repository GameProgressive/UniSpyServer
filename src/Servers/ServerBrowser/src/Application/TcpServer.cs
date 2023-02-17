using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.ServerBrowser.Application
{
    class TcpServer : UniSpy.Server.Core.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public TcpServer(UniSpyServerConfig config) : base(config)
        {
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}