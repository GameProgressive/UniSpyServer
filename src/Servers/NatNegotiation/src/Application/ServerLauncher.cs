using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.NatNegotiation.Application
{
    internal sealed class ServerLauncher : ServerLauncherBase
    {
        public ServerLauncher() : base("NatNegotiation")
        {
        }

        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new UdpServer(config.ServerID, config.ServerName, config.ListeningIPEndPoint);
    }
}