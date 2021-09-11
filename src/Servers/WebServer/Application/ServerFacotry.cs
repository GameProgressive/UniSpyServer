using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.UniSpyConfig;
using WebServer.Network;

namespace WebServer.Application
{
    internal class ServerFactory : ServerFactoryBase
    {
        public ServerFactory()
        {
        }

        protected override void StartServer(UniSpyServerConfig cfg)
        {
            if (cfg.ServerName == ServerName)
            {
                Server = new Server(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}