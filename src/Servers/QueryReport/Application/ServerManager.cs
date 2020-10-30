using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Common;
using GameSpyLib.RetroSpyConfig;
using QueryReport.Server;
using System;
using System.Net;

namespace QueryReport.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class ServerManager : ServerManagerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in config file</param>
        public ServerManager(string serverName) : base(serverName)
        {
        }

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(ServerConfig cfg)
        {

            if (cfg.Name == ServerName)
            {
                // case "GPCM":
                Server = new QRServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort).Start();
                Console.WriteLine(
                    GameSpyLib.Extensions.StringExtensions.FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }

        /// <summary>
        /// Stop a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to stop</param>

    }
}
