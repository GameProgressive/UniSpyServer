using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Database.Entity;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.RetroSpyConfig;
using Serilog.Events;
using StackExchange.Redis;
using System;
using System.Linq;


namespace GameSpyLib.Common
{
    public abstract class ServerManagerBase
    {
        public readonly string RetroSpyVersion = "0.5.1";
        public static string ServerName { get; protected set; }
        public static ConfigManager Config { get; protected set; }
        public static ConnectionMultiplexer Redis { get; protected set; }
        protected object Server;

        protected bool Disposed = false;

        public ServerManagerBase(RetroSpyServerName serverName)
        {
            ServerName = serverName.ToString();
        }

        public bool Start()
        {
            try
            {
                StringExtensions.ShowRetroSpyLogo(RetroSpyVersion);
                LoadDatabaseConfig();
                LoadServerConfig();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void LoadServerConfig()
        {
            LogWriter.ToLog(LogEventLevel.Information, StringExtensions.FormatServerTableHeader("-----------", "--------------", "------"));
            LogWriter.ToLog(LogEventLevel.Information, StringExtensions.FormatServerTableContext("Server Name", "Host Name", "Port"));
            LogWriter.ToLog(LogEventLevel.Information, StringExtensions.FormatServerTableHeader("-----------", "--------------", "------"));
            // Add all servers
            foreach (ServerConfig cfg in ConfigManager.Config.Servers)
            {
                StartServer(cfg);
            }
            LogWriter.ToLog(LogEventLevel.Information, StringExtensions.FormatServerTableHeader("-----------", "--------------", "------"));
            LogWriter.ToLog(LogEventLevel.Information, " Server is successfully started! ");
        }

        protected abstract void StartServer(ServerConfig cfg);

        private void LoadDatabaseConfig()
        {
            DatabaseConfig dbConfig = ConfigManager.Config.Database;
            // Determine which database is using and create the database connection

            switch (ConfigManager.Config.Database.Type)
            {
                case DatabaseEngine.MySql:
                    string mySqlConnStr =
                        string.Format(
                            "Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode={5};SslCert={6};SslKey={7};SslCa={8}",
                            dbConfig.RemoteAddress, dbConfig.DatabaseName, dbConfig.UserName, dbConfig.Password,
                            dbConfig.RemotePort, dbConfig.SslMode, dbConfig.SslCert, dbConfig.SslKey, dbConfig.SslCa);
                    retrospyContext.RetroSpyMySqlConnStr = mySqlConnStr;
                  
                    //using (var db = new retrospyContext())
                    //{
                    //    db.Users.Where(u => u.Userid == 0);
                    //}
                    break;
                case DatabaseEngine.SQLite:
                    string SQLiteConnStr = "Data Source=" + dbConfig.DatabaseName + ";Version=3;New=False";

                    break;
                default:
                    throw new Exception("Unknown database engine!");
            }

            if (!new retrospyContext().Database.CanConnect())
            {
                throw new Exception("Can not connect to database!");
            }

            LogWriter.Log.Information($"Successfully connected to the {dbConfig.Type}!");

            RedisConfig redisConfig = ConfigManager.Config.Redis;
            Redis = ConnectionMultiplexer.Connect(redisConfig.RemoteAddress + ":" + redisConfig.RemotePort.ToString());
            LogWriter.Log.Information($"Successfully connected to Redis!");
        }
    }
}
