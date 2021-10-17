using ConsoleTables;
using StackExchange.Redis;
using System;
using System.Reflection;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Config;
using UniSpyLib.Logging;

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
        public static readonly string ServerName = Assembly.GetEntryAssembly().GetName().Name.Split(".")[2];
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
            Console.Title = "UniSpyServer v" + Version + " - " + ServerName;
        }

        /// <summary>
        /// Overwrite specific functions you want to start the server with
        /// </summary>
        /// <param name="cfg"></param>
        protected abstract void StartServer(UniSpyServerConfig cfg);

        public virtual void Start()
        {
            ServerInfo();
            ConnectDB();
            ConnectRedis();
            LoadServerConfig();
        }

        protected static void ServerInfo()
        {
            LogWriter.Info(@"UniSpyServer version " + Version);
            LogWriter.Info(@"Starting " + ServerName);
        }

        protected void ConnectDB()
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
                LogWriter.Fatal($"{dbConfig.Type}: Connection failed!" + e);
            }
            LogWriter.Info($"{dbConfig.Type}: Connected using Port: " + dbConfig.RemotePort);
        }
        protected void ConnectRedis()
        {
            var redisConfig = ConfigManager.Config.Redis;
            try
            {
                Redis = ConnectionMultiplexer.Connect($"{redisConfig.RemoteAddress}:{redisConfig.RemotePort}");
            }
            catch (Exception e)
            {
                LogWriter.Fatal("Redis: Connection failed!" + e);
            }
            LogWriter.Info($"Redis: Connected using Port: " + redisConfig.RemotePort);
        }
        protected void LoadServerConfig()
        {
            //Add all servers
            foreach (var cfg in ConfigManager.Config.Servers)
            {
                StartServer(cfg);
            }

            if (Server != null)
            {
                Server.Start();
                LogWriter.Info(@"Successfully started!");
                LogWriter.Info(@"Listening on: " + Server.Endpoint.Address + ":" + Server.Endpoint.Port);
            }
        }

    }
}
