using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.GameTrafficRelay.Application
{
    public sealed class ServerLauncher : ServerLauncherBase
    {

        protected override IServer LaunchNetworkService()
        {

            var server = new Server();
            if (server.PublicIPEndPoint.Address.Equals(IPAddress.Any) || server.PublicIPEndPoint.Address.Equals(IPAddress.Loopback))
            {
                throw new System.Exception("Game traffic relay server public address can not set to 0.0.0.0 or 127.0.0.1 !");
            }
            return server;
        }
    }
}