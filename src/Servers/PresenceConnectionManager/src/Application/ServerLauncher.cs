using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    public sealed class ServerLauncher : ServerLauncherBase
    {
        protected override IServer LaunchNetworkService() => new Server();
    }
}