using PresenceConnectionManager.Network;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.Config;

namespace PresenceConnectionManager.Application
{
    /// <summary>
    /// A factory that creates instances of servers
    /// </summary>
    internal sealed class ServerFactory : ServerFactoryBase
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
