using System.Net;
using UniSpy.Server.GameTrafficRelay.Entity;
using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.GameTrafficRelay.Application
{
    internal sealed class ServerLauncher : ServerLauncherBase
    {
        private ServerStatusReporter _serverStatusReporter;

        public ServerLauncher() : base("GameTrafficRelay")
        {
        }
        protected override IServer LaunchNetworkService(UniSpyServerConfig config)
        {
            if (config.PublicIPEndPoint.Address.Equals(IPAddress.Any) || config.PublicIPEndPoint.Address.Equals(IPAddress.Loopback))
            {
                throw new System.Exception("Game traffic relay server public address can not set to 0.0.0.0 or 127.0.0.1 !");
            }
            _serverStatusReporter = new ServerStatusReporter(config);
            return new WebServer(config);

        }
        public override void Start()
        {
            base.Start();
            _serverStatusReporter.Start();
        }
    }
}