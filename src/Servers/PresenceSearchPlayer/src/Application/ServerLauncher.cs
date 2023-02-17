using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.PresenceSearchPlayer.Application
{
    internal sealed class ServerLauncher : ServerLauncherBase
    {
        public ServerLauncher() : base("PresenceSearchPlayer")
        {
        }

        protected override IServer LaunchNetworkService(UniSpyServerConfig config) => new TcpServer(config);
    }
}