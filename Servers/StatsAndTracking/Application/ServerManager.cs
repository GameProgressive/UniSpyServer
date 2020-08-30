using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.RetroSpyConfig;
using System;
using System.Net;

namespace StatsAndTracking.Application
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
                Server = new GStatsServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort).Start();

                Console.WriteLine(
                     StringExtensions.FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));

            }
        }
    }
}
