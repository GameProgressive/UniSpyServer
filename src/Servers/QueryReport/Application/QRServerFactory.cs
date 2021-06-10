using QueryReport.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.MiscMethod;
using UniSpyLib.UniSpyConfig;

namespace QueryReport.Application
{

    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class QRServerFactory : UniSpyServerFactoryBase
    {
        public new static QRServer Server
        {
            get => (QRServer)UniSpyServerFactoryBase.Server;
            private set => UniSpyServerFactoryBase.Server = value;
        }
        public QRServerFactory()
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
                Server = new QRServer(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
