using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.UniSpyConfig;
using NATNegotiation.Network;
using System;
using System.Net;

namespace NATNegotiation.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class NNServerManager : ServerManagerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in config file</param>
        public NNServerManager(string serverName) : base(serverName)
        {
        }

        public override void Start()
        {
            ShowRetroSpyLogo();
            //LoadDatabaseConfig();
            LoadServerConfig();
        }
        /// <summary>
        /// Starts a specific server, you can also start all server in once if you do not check the server name.
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(ServerConfig cfg)
        {
            if (cfg.Name == ServerName)
            {
                Server = new NNServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort).Start();
                Console.WriteLine(
                    StringExtensions.FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
