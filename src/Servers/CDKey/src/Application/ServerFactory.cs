using UniSpyServer.Servers.CDkey.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.CDkey.Application
{
    /// <summary>
    /// A factory that creates instances of servers
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
        /// <param name="cfg">The configuration of the specific server to be run</param>
        protected override void StartServer(UniSpyServerConfig cfg)
        {
            if (cfg.ServerName == ServerName)
            {
                Server = new Server(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
