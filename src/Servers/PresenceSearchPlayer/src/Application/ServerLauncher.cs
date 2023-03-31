using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceSearchPlayer.Application
{
    public sealed class ServerLauncher : ServerLauncherBase
    {
        protected override IServer LaunchNetworkService() => new Server();
    }
}