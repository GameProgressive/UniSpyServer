using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.WebServer.Application
{
    internal sealed class ServerLauncher : ServerLauncherBase
    {
        public ServerLauncher() : base("WebServer")
        {
        }

        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new HttpServer(config.ServerID, config.ServerName, config.ListeningIPEndPoint);
    }
}