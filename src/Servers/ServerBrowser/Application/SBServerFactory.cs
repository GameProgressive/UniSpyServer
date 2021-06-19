using ServerBrowser.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.UniSpyConfig;
using QueryReport.Handler.SystemHandler;

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
        public SBServerFactory()
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
                Server = new SBServer(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
