using CDKey.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.UniSpyConfig;
namespace CDKey.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class CDKeyServerFactory : UniSpyServerFactoryBase
    {
        public static new CDKeyServer Server
        {
            get => (CDKeyServer)UniSpyServerFactoryBase.Server;
            private set => UniSpyServerFactoryBase.Server = value;
        }

        public CDKeyServerFactory() 
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
                Server = new CDKeyServer(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
