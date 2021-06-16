using ServerBrowser.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.UniSpyConfig;

namespace ServerBrowser.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class SBServerFactory : UniSpyServerFactoryBase
    {
        public new static SBServer Server
        {
            get => (SBServer)UniSpyServerFactoryBase.Server;
            private set => UniSpyServerFactoryBase.Server = value;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in config file</param>
        public SBServerFactory()
        {
        }

        /// <summary>
        /// Checks if a specific server is running
        /// </summary>
        /// <param name="cfg">The specific server configuration</param>
        /// <returns>true if the server is running, false if the server is not running or the specified server does not exist</returns>

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(UniSpyServerConfig cfg)
        {
            if (cfg.ServerName == ServerName)
            {
                Server = new SBServer(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
