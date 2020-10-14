using GameSpyLib.Database;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Extensions;
using GameSpyLib.RetroSpyConfig;
using StackExchange.Redis;
using System;


namespace GameSpyLib.Common
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
            Console.WriteLine("Server is successfully started! ");
        }

        /// <summary>
        /// Over write the specific functions you want to start the server
        /// You can start all servers if you want
        /// </summary>
        /// <param name="cfg"></param>
        protected abstract void StartServer(ServerConfig cfg);

        protected void LoadDatabaseConfig()
        {
            DatabaseConfig dbConfig = ConfigManager.Config.Database;
            //Determine which database is used and establish the database connection.

            switch (dbConfig.Type)
            {
                case DatabaseType.MySql:
                    string mySqlConnStr =
                        string.Format(
                            "Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode={5};SslCert={6};SslKey={7};SslCa={8}",
                            dbConfig.RemoteAddress, dbConfig.DatabaseName, dbConfig.UserName, dbConfig.Password,
                            dbConfig.RemotePort, dbConfig.SslMode, dbConfig.SslCert, dbConfig.SslKey, dbConfig.SslCa);
                    retrospyContext.RetroSpyMySqlConnStr = mySqlConnStr;
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
            Console.WriteLine($"Successfully connected to {dbConfig.Type}!");

            try
            {
                RedisConfig redisConfig = ConfigManager.Config.Redis;
                Redis = ConnectionMultiplexer.Connect(redisConfig.RemoteAddress + ":" + redisConfig.RemotePort.ToString());
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
