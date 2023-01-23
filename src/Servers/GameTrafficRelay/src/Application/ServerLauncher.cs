using System.Net;
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
            if (config.ListeningEndPoint.Address.ToString() == "0.0.0.0" || config.ListeningEndPoint.Address.ToString() == "127.0.0.1")
            {
                throw new System.Exception("Game traffic relay server can not listen to loopback endpoint!");
            }
            return new UdpServer(config.ServerID, config.ServerName, config.ListeningEndPoint);
        }

        public override void Start()
        {
            base.Start();
            _serverStatusReporter.Start();
        }
    }
}