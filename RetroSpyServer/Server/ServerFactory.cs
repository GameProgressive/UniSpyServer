using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using GameSpyLib.Database;
using GameSpyLib.Network;

namespace RetroSpyServer
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public class ServerFactory : IDisposable
    {
        protected Dictionary<string, TemplateServer> servers;

        private DatabaseDriver databaseDriver = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServerFactory()
        {
            servers = new Dictionary<string, TemplateServer>();
        }

        /// <summary>
        /// Default decostructor
        /// </summary>
        ~ServerFactory()
        {
            Dispose();
        }

        /// <summary>
        /// Creates the server factory
        /// </summary>
        /// <param name="engine">The database engine</param>
        public void Create(DatabaseEngine engine, string connectionString)
        {
            switch (engine)
            {
                case DatabaseEngine.Mysql:
                    databaseDriver = new MySqlDatabaseDriver(connectionString);
                    break;
                case DatabaseEngine.Sqlite:
                    databaseDriver = new SqliteDatabaseDriver(connectionString);
                    break;
                default:
                    throw new Exception("Unknown database engine!");
            }

            databaseDriver.Connect();

            CreateDatabaseTables();

            // Add all servers
            servers.Add("GPSP", new GPSPServer(databaseDriver));
        }

        /// <summary>
        /// Free all resources created by the server factory
        /// </summary>
        public void Dispose()
        {
            StopAllServers();
            
            foreach (KeyValuePair<string, TemplateServer> server in servers)
                server.Value.Dispose();

            databaseDriver.Dispose();
        }

        /// <summary>
        /// Stop all servers included in the server factory
        /// </summary>
        public void StopAllServers()
        {
            foreach (KeyValuePair<string, TemplateServer> server in servers)
                server.Value.Stop();
        }

        /// <summary>
        /// Checks if a spefici server is running
        /// </summary>
        /// <param name="serverName">The specific server name</param>
        /// <returns></returns>
        public bool IsServerRunning(string serverName)
        {
            serverName = serverName.ToUpper();

            if (!servers.ContainsKey(serverName))
                return false;

            return servers[serverName].IsRunning;
        }

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="serverName">The specific server name</param>
        /// <param name="ip">The IP that will be binded</param>
        /// <param name="port">The port that will be binded</param>
        /// <param name="maxConnections">Max number of connections</param>
        public void StartServer(string serverName, string ip, int port, int maxConnections)
        {
            serverName = serverName.ToUpper();

            if (!servers.ContainsKey(serverName))
                return;

            if (servers[serverName] == null)
                return;

            servers[serverName].Start(ip, port, maxConnections);
        }

        /// <summary>
        /// Starts a specific server
        /// </summary>
        /// <param name="serverName">The name of the server</param>
        /// <param name="endPoint">The IP endpoint</param>
        /// <param name="maxConnections">Max number of connections</param>
        public void StartServer(string serverName, IPEndPoint endPoint, int maxConnections)
        {
            serverName = serverName.ToUpper();

            if (!servers.ContainsKey(serverName))
                return;

            if (servers[serverName] == null)
                return; // A server istance cannot be null

            servers[serverName].Start(endPoint, maxConnections);
        }

        /// <summary>
        /// Stop a specific server
        /// </summary>
        /// <param name="serverName">The name of the server</param>
        public void StopServer(string serverName)
        {
            serverName = serverName.ToUpper();

            if (!servers.ContainsKey(serverName))
                return;

            servers[serverName].Stop();
        }


        /// <summary>
        /// Indicates if the server is running or not
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            foreach (KeyValuePair<string, TemplateServer> server in servers)
            {
                if (server.Value.IsRunning)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Creates the tables on the database
        /// </summary>
        private void CreateDatabaseTables()
        {
            databaseDriver.Query(
                @"CREATE TABLE IF NOT EXISTS `users` (" +
                @"`id` INTEGER(11) NOT NULL PRIMARY KEY," +
                @"`email` VARCHAR(50) NOT NULL," +
                @"`password` VARCHAR(32) NOT NULL," +
                @"`status` INTEGER(1) NOT NULL DEFAULT '0'" +
                @")"
            );

            databaseDriver.Query(@"INSERT INTO users(id, email, password, status) VALUES(1, 'spyguy@gamespy.com', '0000', 1)");
        }
    }
}
