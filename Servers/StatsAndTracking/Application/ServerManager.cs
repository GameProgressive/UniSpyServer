using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
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
        /// <param name="serverName">Server name in XML config file</param>
        public ServerManager(string serverName) : base(serverName)
        {
        }

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to run</param>
        protected override void StartServer(GameSpyLib.RetroSpyConfig.ServerConfig cfg)
        {
            //if (cfg.Disabled)
            //    return;            
            //LogWriter.Log.Write("Starting {2} server at  {0}:{1}.", LogLevel.Info, cfg.Hostname, cfg.Port, cfg.Name);
            //LogWriter.Log.Write("Maximum connections for {0} are {1}.", LogLevel.Info, cfg.Name, cfg.MaxConnections);
            if (cfg.Name == ServerName)
            {
                // case "GPCM":
                Server = new GStatsServer(IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort);
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Information,
                     StringExtensions.FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }
    }
}
