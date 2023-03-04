using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.QueryReport.Application
{
    internal sealed class ServerLauncher : ServerLauncherBase
    {
        public ServerLauncher() : base("QueryReport")
        {
        }

        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new Server(config);
    }
}