using System.Threading.Tasks;
using NatNegotiation.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.MiscMethod;
using UniSpyLib.UniSpyConfig;

namespace NatNegotiation.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    internal sealed class NNServerFactory : UniSpyServerFactoryBase
    {
        public new static NNServer Server
        {
            get => (NNServer)UniSpyServerFactoryBase.Server;
            private set => UniSpyServerFactoryBase.Server = value;
        }

        public NNServerFactory()
        {
        }
        /// <summary>
        /// NatNeg server do not need to access to MySql database
        /// </summary>
        public override async Task Start()
        {
            await ShowUniSpyLogo();
            await LoadUniSpyRequests();
            await LoadUniSpyHandlers();
            await ConnectRedis();
            await LoadServerConfig();
            await UniSpyJsonConverter.Initialize();
        }

        /// <summary>
        /// Starts a specific server, you can also start all server in once if you do not check the server name.
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(UniSpyServerConfig cfg)
        {
            if (cfg.ServerName == ServerName)
            {
                Server = new NNServer(cfg.ServerID, cfg.ListeningEndPoint);
            }
        }
    }
}
