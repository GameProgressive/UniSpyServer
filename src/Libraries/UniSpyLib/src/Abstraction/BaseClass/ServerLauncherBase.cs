using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory
{
    public abstract class ServerLauncherBase
    {
        /// <summary>
        /// UniSpy server version
        /// </summary>
        public static readonly string Version = "0.7.3";
        /// <summary>
        /// UniSpy server name
        /// </summary>
        /// <returns></returns>
        public static string ServerName { get; protected set; }
        public static Dictionary<string, IServer> Servers { get; protected set; }
        public static IServer Server;
        static ServerLauncherBase()
        {
            Servers = new Dictionary<string, IServer>();
            StackExchange.Redis.ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
        }
        public ServerLauncherBase(string serverNames)
        {
            ServerName = serverNames;
        }

        public virtual void Start()
        {
            Console.Title = $"UniSpyServer  {Version} - {ServerName}";
            ShowUniSpyLogo();
            ConnectMySql();
            ConnectRedis();
            LaunchServer();
        }
        protected abstract IServer LaunchNetworkService(UniSpyServerConfig config);
        protected virtual void LaunchServer()
        {
            var cfg = ConfigManager.Config.Servers.Where(s => s.ServerName == ServerName).First();
            Server = LaunchNetworkService(cfg);
            
            if (Server is null)
            {
                throw new Exception("Server created failed");
            }
            // asp.net web server does not implement a Server interface, therefore this code should not be called
            Server.Start();
            var table = new ConsoleTable("Server Name", "Listening Address", "Listening Port");
            table.AddRow(ServerName, Server.ListeningEndPoint.Address, Server.ListeningEndPoint.Port);
            table.Write(ConsoleTables.Format.Alternative);
            Servers.Add(cfg.ServerName, Server);
            Console.WriteLine("Server successfully started!");
        }

        protected void ConnectRedis()
        {
            var redisConfig = ConfigManager.Config.Redis;
            try
            {
                StackExchange.Redis.ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
            }
            catch (Exception e)
            {
                throw new Exception("Can not connect to Redis", e);
            }
            Console.WriteLine($"Successfully connected to Redis at {redisConfig.Server}:{redisConfig.Port}");
        }
        protected void ConnectMySql()
        {
            //Determine which database is used and establish the database connection.
            var dbConfig = ConfigManager.Config.Database;
            try
            {
                new UniSpyContext().Database.CanConnect();
            }
            catch (Exception e)
            {
                throw new Exception($"Can not connect to {dbConfig.Type}!", e);
            }

            Console.WriteLine($"Successfully connected to {dbConfig.Type} at {dbConfig.Server}:{dbConfig.Port}");
        }
        protected static void ShowUniSpyLogo()
        {
            // the ascii art font name is "small"
            Console.WriteLine(@"");
            Console.WriteLine(@" _   _      _ ___           ___ ");
            Console.WriteLine(@"| | | |_ _ (_) __|_ __ _  _/ __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@"| |_| | ' \| \__ \ '_ \ || \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@" \___/|_||_|_|___/ .__/\_, |___/\___|_|  \_/\___|_|");
            Console.WriteLine(@"                 |_|   |__/ ");
            Console.WriteLine(@"Version: " + Version);
        }
    }
}
