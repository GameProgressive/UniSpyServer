using GameSpyLib.Database;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Extensions;
using GameSpyLib.RetroSpyConfig;
using StackExchange.Redis;
using System;


namespace GameSpyLib.Abstraction.BaseClass
{
    public abstract class ServerManagerBase
    {
        public static readonly string RetroSpyVersion = "0.5.2";
        public static string ServerName { get; protected set; }
        public static ConnectionMultiplexer Redis { get; protected set; }
        protected object Server;

        public ServerManagerBase(string serverName)
        {
            ServerName = serverName;
        }

        public virtual void Start()
        {
            ShowRetroSpyLogo();
            LoadDatabaseConfig();
            LoadServerConfig();
        }

        protected void LoadServerConfig()
        {
            Console.WriteLine(StringExtensions.FormatServerTableHeader("-----------", "--------------", "------"));
            Console.WriteLine(StringExtensions.FormatServerTableContext("Server Name", "Host Name", "Port"));
            Console.WriteLine(StringExtensions.FormatServerTableHeader("-----------", "--------------", "------"));
            //Add all servers
            foreach (ServerConfig cfg in ConfigManager.Config.Servers)
            {
                StartServer(cfg);
            }
            Console.WriteLine(StringExtensions.FormatServerTableHeader("-----------", "--------------", "------"));
            Console.WriteLine("Server successfully started! ");
        }

        /// <summary>
        /// Over write the specific functions you want to start the server
        /// You can start all servers if you want
        /// </summary>
        /// <param name="cfg"></param>
        protected abstract void StartServer(ServerConfig cfg);

        protected void LoadDatabaseConfig()
        {
            //Determine which database is used and establish the database connection.

            switch (ConfigManager.Config.Database.Type)
            {
                case DatabaseType.MySql:
                    retrospyContext.RetroSpyMySqlConnStr =
                    $"Server={ConfigManager.Config.Database.RemoteAddress};"
                    + $"Database={ConfigManager.Config.Database.DatabaseName};"
                    + $"Uid={ConfigManager.Config.Database.UserName};"
                    + $"Pwd={ConfigManager.Config.Database.Password};"
                    + $"Port={ConfigManager.Config.Database.RemotePort};"
                    + $"SslMode={ConfigManager.Config.Database.SslMode};"
                    + $"SslCert={ConfigManager.Config.Database.SslCert};"
                    + $"SslKey={ConfigManager.Config.Database.SslKey};"
                    + $"SslCa={ConfigManager.Config.Database.SslCa}";
                    break;
            }

            try
            {
                new retrospyContext().Database.CanConnect();
            }
            catch (Exception e)
            {
                throw new Exception($"Can not connect to {ConfigManager.Config.Database.Type}!", e);
            }

            Console.WriteLine($"Successfully connected to {ConfigManager.Config.Database.Type}!");

            try
            {
                Redis = ConnectionMultiplexer.Connect(
                    $"{ConfigManager.Config.Redis.RemoteAddress}:{ConfigManager.Config.Redis.RemotePort}");
            }
            catch (Exception e)
            {
                throw new Exception("Can not connect to Redis", e);
            }
            Console.WriteLine($"Successfully connected to Redis!");
        }

        public static void ShowRetroSpyLogo()
        {
            Console.WriteLine(@" ___     _           ___             ___                      ");
            Console.WriteLine(@"| _ \___| |_ _ _ ___/ __|_ __ _  _  / __| ___ _ ___ _____ _ _ ");
            Console.WriteLine(@"|   / -_)  _| '_/ _ \__ \ '_ \ || | \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine(@"|_|_\___|\__|_| \___/___/ .__/\_, | |___/\___|_|  \_/\___|_|  ");
            Console.WriteLine(@"                        |_|   |__/                            ");
            Console.WriteLine(@"Version: " + RetroSpyVersion);
        }
    }
}
