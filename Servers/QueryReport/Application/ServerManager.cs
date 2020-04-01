using GameSpyLib.Common;
using GameSpyLib.Logging;
using GameSpyLib.RetroSpyConfig;
using QueryReport.Server;
using System.Net;

namespace QueryReport.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class ServerManager : ServerManagerBase
    {
        private QRServer Server = null;

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
                // case "GPCM":
                Server = new QRServer(cfg.Name, DBEngine, IPAddress.Parse(cfg.ListeningAddress), cfg.ListeningPort);
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Information,
                    GameSpyLib.Extensions.StringExtensions.FormatServerTableContext(cfg.Name, cfg.ListeningAddress, cfg.ListeningPort.ToString()));
            }
        }

        /// <summary>
        /// Stop a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to stop</param>
        protected override void StopServer()
        {
            Server?.Dispose();
        }
    }
}
