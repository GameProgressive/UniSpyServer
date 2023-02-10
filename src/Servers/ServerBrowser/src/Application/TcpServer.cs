using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    class TcpServer : UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public TcpServer(UniSpyServerConfig config) : base(config)
        {
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}