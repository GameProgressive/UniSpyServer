using ConsoleTables;
using StackExchange.Redis;
using System;
using System.Reflection;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyServer.UniSpyLib.Config;
using System.Linq;
using System.Collections.Generic;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory
{
    public abstract class ServerFactoryBase
    {
        /// <summary>
        /// UniSpy server version
        /// </summary>
        public static readonly string Version = "0.5.7";
        /// <summary>
        /// UniSpy server name
        /// </summary>
        /// <returns></returns>
        public static string ServerName { get; protected set; }
        /// <summary>
        /// Redis connection
        /// </summary>
        public static ConnectionMultiplexer Redis { get; protected set; }
        /// <summary>
        /// A UniSpyServer instance
        /// </summary>
        public static IUniSpyServer Server { get; protected set; }
        public ServerFactoryBase()
        {
        }
        private List<string> _availableServers = new List<string>();
        public ServerFactoryBase(List<string> servers)
        {
            _availableServers = servers;
        }

        public virtual void Start()
        {
            Console.Title = $"UniSpyServer  {Version} - {ServerName}";
            ShowUniSpyLogo();
            ConnectMySql();
            ConnectRedis();
            LoadServerConfig();
        }
        protected void LoadServerConfig()
        {
            var cfg = ConfigManager.Config.Servers.Where(s => s.ServerName == ServerName).First();
            var assembly = Assembly.Load($"UniSpyServer.Servers.{ServerName}");
            var type = assembly.GetType($"UniSpyServer.Servers.{ServerName}.Network.Server");

            Server = (IUniSpyServer)Activator.CreateInstance(type, cfg.ServerID, cfg.ListeningEndPoint);

            if (Server == null)
            {
                throw new Exception("Server created failed");
            }
            // asp.net web server does not implement a Server interface, therefore this code should not be called
            Server.Start();
            var table = new ConsoleTable("Server Name", "Listening Address", "Listening Port");
            table.AddRow(ServerName, Server.Endpoint.Address, Server.Endpoint.Port);
            table.Write(ConsoleTables.Format.Alternative);
            Console.WriteLine("Server successfully started!");
        }

        protected void ConnectRedis()
        {
            var redisConfig = ConfigManager.Config.Redis;
            try
            {
                Redis = ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
            }
            catch (Exception e)
            {
                throw new Exception("Can not connect to Redis", e);
            }
            Console.WriteLine($"Successfully connected to Redis at {redisConfig.RemoteAddress}:{redisConfig.RemotePort}");
        }
        protected void ConnectMySql()
        {
            //Determine which database is used and establish the database connection.
            var dbConfig = ConfigManager.Config.Database;
            try
            {
                new UnispyContext().Database.CanConnect();
            }
            catch (Exception e)
            {
                throw new Exception($"Can not connect to {dbConfig.Type}!", e);
            }

            Console.WriteLine($"Successfully connected to {dbConfig.Type} at {dbConfig.RemoteAddress}:{dbConfig.RemotePort}");
        }
        protected static void ShowUniSpyLogo()
        {
            // the ascii art font name is "small"
            Console.WriteLine(@" _   _      _ ___           ___ ");
            Console.WriteLine(@"| | | |_ _ (_) __|_ __ _  _/ __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@"| |_| | ' \| \__ \ '_ \ || \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" \___/|_||_|_|___/ .__/\_, |___/\___|_|  \_/\___|_|");
            Console.WriteLine(@"                 |_|   |__/ ");
            Console.WriteLine(@"Version: " + Version);
        }
    }
}
