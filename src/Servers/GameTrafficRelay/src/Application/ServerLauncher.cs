using UniSpyServer.Servers.GameTrafficRelay.Entity.Structure.Redis;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.GameTrafficRelay.Application
{
    public class ServerLauncher : ServerLauncherBase
    {
        private ServerStatusReporter _serverStatusReporter;

        public ServerLauncher() : base("GameTrafficRelay")
        {
        }

        protected override IServer LaunchNetworkService(UniSpyServerConfig config)
        {
            _serverStatusReporter = new ServerStatusReporter(config);
            return new UdpServer(config.ServerID, config.ServerName, config.ListeningEndPoint);
        }

        public override void Start()
        {
            base.Start();
            _serverStatusReporter.Start();
        }
    }
}