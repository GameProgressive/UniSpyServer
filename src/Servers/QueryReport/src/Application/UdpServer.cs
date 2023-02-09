using UniSpyServer.Servers.QueryReport.Application;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.QueryReport.V2.Application
{
    internal sealed class UdpServer : UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Udp.Server.UdpServer
    {
        public UdpServer(UniSpyServerConfig config) : base(config)
        {
        }
        public override void Start()
        {
            base.Start();
            StorageOperation.Channel.StartSubscribe();
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}