using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.ServerBrowser.Application
{
    internal sealed class ServerLauncher : ServerLauncherBase
    {
        public ServerLauncher() : base("ServerBrowser")
        {
        }
        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new TcpServer(config);
    }
}