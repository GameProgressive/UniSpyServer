using GameStatus.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.UniSpyConfig;

namespace GameStatus.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class GSServerFactory : ServerFactoryBase
    {
        public static new GSServer Server
        {
            get => (GSServer)ServerFactoryBase.Server;
            private set => ServerFactoryBase.Server = value;
        }        /// <summary>
                 /// Constructor
                 /// </summary>
                 /// <param name="serverName">Server name in config file</param>
        public GSServerFactory()
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
                Server = new GSServer(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
