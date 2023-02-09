using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.CDKey.Application
{
    class UdpServer : UniSpyLib.Abstraction.BaseClass.Network.Udp.Server.UdpServer
    {
        public UdpServer(UniSpyServerConfig config) : base(config)
        {
        }

        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}