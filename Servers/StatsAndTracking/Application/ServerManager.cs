using GameSpyLib.Common;
using GameSpyLib.Logging;
using GameSpyLib.XMLConfig;
using System.Net;

namespace StatsAndTracking.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class ServerManager : ServerManagerBase
    {


        private GStatsServer Server = null;

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
        protected override void StartServer(ServerConfiguration cfg)
        {
            //if (cfg.Disabled)
            //    return;            
            //LogWriter.Log.Write("Starting {2} server at  {0}:{1}.", LogLevel.Info, cfg.Hostname, cfg.Port, cfg.Name);
            //LogWriter.Log.Write("Maximum connections for {0} are {1}.", LogLevel.Info, cfg.Name, cfg.MaxConnections);
            if (cfg.Name == ServerName)
            {
                // case "GPCM":
                Server = new GStatsServer(cfg.Name, DBEngine, IPAddress.Parse(cfg.Hostname), cfg.Port);
                LogWriter.Log.Write(LogLevel.Info, "|{0,-11}|{1,-14}|{2,-6}|", cfg.Name, cfg.Hostname, cfg.Port);
            }
        }


        /// <summary>
        /// Stop a specific server
        /// </summary>
        /// <param name="cfg">The configuration of the specific server to stop</param>
        protected override void StopServer()
        {
            Server?.Stop();
        }
    }
}
