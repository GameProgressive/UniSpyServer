using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.PresenceConnectionManager.Application
{
    public class ServerLauncher : ServerLauncherBase
    {
        public ServerLauncher() : base("PresenceConnectionManager")
        {
        }

        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new TcpServer(config.ServerID, config.ServerName, config.ListeningEndPoint);
    }
}