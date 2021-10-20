using UniSpyServer.QueryReport.Network;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;

namespace UniSpyServer.QueryReport.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public sealed class ServerFactory : ServerFactoryBase
    {
        public new static Server Server
        {
            get => (Server)ServerFactoryBase.Server;
            private set => ServerFactoryBase.Server = value;
        }
        public ServerFactory()
        {
        }
        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(UniSpyServerConfig cfg)
        {
            if (cfg.ServerName == ServerName)
            {
                Server = new Server(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
