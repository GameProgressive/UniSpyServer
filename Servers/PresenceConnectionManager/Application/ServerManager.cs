using System;
using System.Net;
using GameSpyLib.Common;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.XMLConfig;


namespace PresenceConnectionManager
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class ServerManager : ServerManagerBase
    {
        private DatabaseDriver databaseDriver = null;

        private GPCMServer Server = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server name in XML config file</param>
        public ServerManager(string serverName) : base(serverName)
        {
        }

        /// <summary>
        /// Checks if a specific server is running
        /// </summary>
        /// <param name="cfg">The specific server configuration</param>
        /// <returns>true if the server is running, false if the server is not running or the specified server does not exist</returns>

        public override bool IsServerRunning()
        {
            return Server != null && !Server.Disposed;
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
                Server = new GPCMServer(cfg.Name, databaseDriver, new IPEndPoint(IPAddress.Parse(cfg.Hostname), cfg.Port), cfg.MaxConnections);
                LogWriter.Log.Write(LogLevel.Info, "|{0,-11}|{1,-14}|{2,-6}|{3,14}|", cfg.Name, cfg.Hostname, cfg.Port, cfg.MaxConnections);
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
