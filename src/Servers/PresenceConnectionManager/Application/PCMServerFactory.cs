using PresenceConnectionManager.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.UniSpyConfig;

namespace PresenceConnectionManager.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class PCMServerFactory : UniSpyServerFactoryBase
    {
        internal new static PCMServer Server
        {
            get => (PCMServer)UniSpyServerFactoryBase.Server;
            private set => UniSpyServerFactoryBase.Server = value;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in config file</param>
        public PCMServerFactory()
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
                Server = new PCMServer(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
