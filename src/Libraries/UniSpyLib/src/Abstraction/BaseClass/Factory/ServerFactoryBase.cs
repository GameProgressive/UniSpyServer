using ConsoleTables;
using StackExchange.Redis;
using System;
using System.Reflection;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Config;

namespace UniSpyLib.Abstraction.BaseClass.Factory
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
        public static string ServerName
        {
            get
            {
                try
                {
                    return Assembly.GetEntryAssembly().GetName().Name.Split(".")[2];
                }
                catch
                {
                    return "test";
                }
            }
        }
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
            Console.Title = $"UniSpyServer  {Version} - {ServerName}";
        }

        public virtual void Start()
        {
            ShowUniSpyLogo();
            ConnectMySql();
            ConnectRedis();
            LoadServerConfig();
        }
        protected void LoadServerConfig()
        {
            //Add all servers
            foreach (var cfg in ConfigManager.Config.Servers)
            {
                StartServer(cfg);
            }

            if (Server != null)
            // asp.net web server does not implement a Server interface, therefore this code should not be called
            {
                Server.Start();
                var table = new ConsoleTable("Server Name", "Listening Address", "Listening Port");
                table.AddRow(ServerName, Server.Endpoint.Address, Server.Endpoint.Port);
                table.Write(ConsoleTables.Format.Alternative);
                Console.WriteLine("Server successfully started!");
            }
        }

        /// <summary>
        /// Over write the specific functions you want to start the server
        /// You can start all servers if you want
        /// </summary>
        /// <param name="cfg"></param>
        protected abstract void StartServer(UniSpyServerConfig cfg);

        protected void ConnectRedis()
        {
            var redisConfig = ConfigManager.Config.Redis;
            try
            {
                Redis = ConnectionMultiplexer.Connect($"{redisConfig.RemoteAddress}:{redisConfig.RemotePort}");
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
            switch (dbConfig.Type)
            {
                case DatabaseType.MySql:
                    unispyContext.UniSpyMySqlConnStr =
                    $"Server={dbConfig.RemoteAddress};"
                    + $"Database={dbConfig.DatabaseName};"
                    + $"Uid={dbConfig.UserName};"
                    + $"Pwd={dbConfig.Password};"
                    + $"Port={dbConfig.RemotePort};"
                    + $"SslMode={dbConfig.SslMode};"
                    + $"SslCert={dbConfig.SslCert};"
                    + $"SslKey={dbConfig.SslKey};"
                    + $"SslCa={dbConfig.SslCa}";
                    break;
            }

            try
            {
                new unispyContext().Database.CanConnect();
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
