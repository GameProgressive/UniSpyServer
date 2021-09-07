using CDKey.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyLib.UniSpyConfig;
namespace CDKey.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class ServerFactory : ServerFactoryBase
    {
        public static new Server Server
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
