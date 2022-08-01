using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.GameStatus.Application
{
    public class ServerLauncher : ServerLauncherBase
    {
        public ServerLauncher() : base("GameStatus")
        {
        }

        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new TcpServer(config.ServerID, config.ServerName, config.ListeningEndPoint);
    }
}