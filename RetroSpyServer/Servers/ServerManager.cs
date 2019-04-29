using System;
using System.Net;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using RetroSpyServer.Config;
using RetroSpyServer.Application;

namespace RetroSpyServer.Servers
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class ServerManager : IDisposable
    {
        private DatabaseDriver databaseDriver = null;

        private GPSP.GPSPServer gpspServer = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServerManager()
        {
            Create();
            
        }

        /// <summary>
        /// Default deconstructor
        /// </summary>
        ~ServerManager()
        {
            Dispose();
        }

        /// <summary>
        /// Creates the server factory
        /// </summary>
        /// <param name="engine">The database engine to create</param>
        public void Create()
        {
            DatabaseConfigurationArttributes cfg = Application.ConfigManager.GetXMLConfiguration().Database;

            // Determine which database is using and create the database connection
            switch (cfg.Type)
            {
                case DatabaseEngine.Mysql:
                    break; // We don't need to create the connection here because each server will automaticly create it's own MySQL connection.
                case DatabaseEngine.Sqlite:
                    databaseDriver = new SqliteDatabaseDriver("Data Source=" + cfg.Name + ";Version=3;New=False");
                    break;
                default:
                    throw new Exception("Unknown database engine!");
            }

            try
            {
                databaseDriver?.Connect();
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Fatal);
                throw ex;
            }

            LogWriter.Log.Write("Successfully connected to the database!", LogLevel.Info);

            // Add all servers
            StartServer("GPSP", 29901);
        }

        /// <summary>
        /// Free all resources created by the server factory
        /// </summary>
        public void Dispose()
        {
            StopAllServers();

            databaseDriver?.Dispose();
        }

        /// <summary>
        /// Stop all servers included in the server factory
        /// </summary>
        public void StopAllServers()
        {
            StopServer("GPSP");
        }

        /// <summary>
        /// Checks if a specific server is running
        /// </summary>
        /// <param name="serverName">The specific server name</param>
        /// <returns>true if the server is running, false if the server is not running or the specified server does not exist</returns>
        public bool IsServerRunning(string serverName)
        {
            serverName = serverName.ToUpper();

            switch (serverName)
            {
                case "GPSP":
                    return gpspServer != null && !gpspServer.IsDisposed;
            }

            return false;
        }

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="serverName">The specific server name</param>
        /// <param name="defaultPort">A default port if no port is specified</param>
        public void StartServer(string serverName, int defaultPort)
        {
            ServerConfigurationArttributes cfg = Application.ConfigManager.GetServerConfiguration(serverName);

            int Port = cfg.Port;
            if (Port == 0)
                Port = defaultPort;

            LogWriter.Log.Write("Starting {2} Player Server at {0}:{1}...", LogLevel.Info, cfg.Hostname, Port, serverName);
            LogWriter.Log.Write("Maximum connections allowed for server {0} are {1}.", LogLevel.Info, serverName, cfg.MaxConnections);

            switch (serverName)
            {
                case "GPSP":
                    gpspServer = new GPSP.GPSPServer(databaseDriver, new IPEndPoint(IPAddress.Parse(cfg.Hostname), cfg.Port), cfg.MaxConnections);
                    break;
            }
        }

        /// <summary>
        /// Stop a specific server
        /// </summary>
        /// <param name="serverName">The name of the server</param>
        public void StopServer(string serverName)
        {
            serverName = serverName.ToUpper();

            switch (serverName)
            {
                case "GPSP":
                    gpspServer?.Dispose();
                    break;
            }
        }


        /// <summary>
        /// Indicates if the server is running or not
        /// </summary>
        /// <returns>If any server is running it returns true, otherwise false</returns>
        public bool IsRunning()
        {
            if (IsServerRunning("GPSP"))
                return true;

            return false;
        }
    }
}
