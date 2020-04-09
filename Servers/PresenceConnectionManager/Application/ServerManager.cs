﻿using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.RetroSpyConfig;
using System.Net;

namespace PresenceConnectionManager.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class ServerManager : ServerManagerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in XML config file</param>
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
                Server = new GPCMServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort);
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Information,
                    StringExtensions.FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }

        /// <summary>
        /// Stop a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to stop</param>

    }
}
