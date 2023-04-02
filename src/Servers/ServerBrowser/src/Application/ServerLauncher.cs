using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.Application
{
    public sealed class ServerLauncher : ServerLauncherBase
    {
        public static IServer ServerV1 => ServerInstances[0];
        public static IServer ServerV2 => ServerInstances[1];
        protected override List<IServer> LaunchNetworkService() =>
        new List<IServer>
        {
            new ServerBrowser.V1.Application.Server(),
            new ServerBrowser.V2.Application.Server()
        };
    }
}