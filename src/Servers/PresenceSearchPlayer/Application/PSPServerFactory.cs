using PresenceSearchPlayer.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.UniSpyConfig;

namespace PresenceSearchPlayer
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class PSPServerFactory : ServerFactoryBase
    {
        public new static PSPServer Server
        {
            get => (PSPServer)ServerFactoryBase.Server;
            private set => ServerFactoryBase.Server = value;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in config file</param>
        public PSPServerFactory()
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
                Server = new PSPServer(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
