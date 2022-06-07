using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.QueryReport.Application
{
    public class ServerLauncher : ServerLauncherBase
    {
        public ServerLauncher() : base("QueryReport")
        {
        }

        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new UdpServer(config.ServerID, config.ServerName, config.ListeningEndPoint);
    }
}