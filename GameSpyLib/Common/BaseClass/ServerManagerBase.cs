
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Database.Entity;
using GameSpyLib.Logging;
using GameSpyLib.XMLConfig;
using System;
using System.IO;
using System.Linq;

namespace GameSpyLib.Common
{
    public abstract class ServerManagerBase : IDisposable
    {
        public readonly string Version = "0.5";
        public static string BasePath { get; protected set; }
        public string LogPath { get; protected set; }
        public string ServerName { get; protected set; }

        public DatabaseEngine DBEngine;

        protected bool Disposed = false;

        public ServerManagerBase(string serverName)
        {
            BasePath = AppDomain.CurrentDomain.BaseDirectory;
            ServerName = serverName;
            LoadLogWriter();
            ShowRetroSpyLogo();
            LoadDatabaseConfig();
            LoadServerConfig();
        }

        private void LoadLogWriter()
        {
            #region Path Setting
            if (!Directory.Exists(BasePath))
                Directory.CreateDirectory(BasePath);

            LogPath = BasePath + @"/Logs/";

            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);
            #endregion
            string logFileName = string.Format("{0}_{1}.log", ServerName, DateTime.Now.ToString("yyyy-MM-dd__HH_mm_ss"));
            LogWriter.Log = new LogWriter(Path.Combine(LogPath, logFileName));
            ConfigManager.Load();
            //set the loglevel to system
            LogWriter.Log.MiniumLogLevel = ConfigManager.xmlConfiguration.LogLevel;
        }

        private void ShowRetroSpyLogo()
        {

            Console.WriteLine("\t" + @"  ___     _           ___             ___                      ");
            Console.WriteLine("\t" + @" | _ \___| |_ _ _ ___/ __|_ __ _  _  / __| ___ _ ___ _____ _ _ ");
            Console.WriteLine("\t" + @" |   / -_)  _| '_/ _ \__ \ '_ \ || | \__ \/ -_) '_\ V / -_) '_|");
            Console.WriteLine("\t" + @" |_|_\___|\__|_| \___/___/ .__/\_, | |___/\___|_|  \_/\___|_|  ");
            Console.WriteLine("\t" + @"                         |_|   |__/                            ");
            Console.WriteLine("");

            LogWriter.Log.Write("RetroSpy Server version " + Version + ".", LogLevel.Info);
        }

        public void LoadServerConfig()
        {
            LogWriter.Log.Write(LogLevel.Info, "+{0,-11}+{1,-14}+{2,-6}+", "-----------", "--------------", "------");
            LogWriter.Log.Write(LogLevel.Info, "|{0,-11}|{1,-14}|{2,-6}|", "Server Name", "Host Name", "Port");
            LogWriter.Log.Write(LogLevel.Info, "+{0,-11}+{1,-14}+{2,-6}+", "-----------", "--------------", "------");
            // Add all servers
            foreach (ServerConfiguration cfg in ConfigManager.xmlConfiguration.Servers)
            {
                StartServer(cfg);
            }
            LogWriter.Log.Write(LogLevel.Info, "+{0,-11}+{1,-14}+{2,-6}+", "-----------", "--------------", "------");
            LogWriter.Log.Write("Server is successfully started! ", LogLevel.Info);
        }


        protected abstract void StartServer(ServerConfiguration cfg);

        protected abstract void StopServer();

        //public abstract bool IsServerRunning();

        private void LoadDatabaseConfig()
        {
            DatabaseConfiguration dbConfiguration = ConfigManager.xmlConfiguration.Database;
            DBEngine = dbConfiguration.Type;
            // Determine which database is using and create the database connection

            switch (dbConfiguration.Type)
            {
                case DatabaseEngine.MySql:
                    string mySqlConnStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode={5};SslCert={6};SslKey={7};SslCa={8}", dbConfiguration.Hostname, dbConfiguration.Databasename, dbConfiguration.Username, dbConfiguration.Password, dbConfiguration.Port, dbConfiguration.SslMode, dbConfiguration.SslCert, dbConfiguration.SslKey, dbConfiguration.SslCa);
                    retrospyContext.RetroSpyMySqlConnStr = mySqlConnStr;
                    using (var db = new retrospyContext())
                    {
                        db.Users.Select(u => u).Count();
                    }
                    break;
                case DatabaseEngine.SQLite:
                    string SQLiteConnStr = "Data Source=" + dbConfiguration.Databasename + ";Version=3;New=False";
                    
                    break;
                default:
                    throw new Exception("Unknown database engine!");
            }
            LogWriter.Log.Write(LogLevel.Info, "Successfully connected to the {0}!", dbConfiguration.Type);
        }


        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)。                
                StopServer();
            }

            // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
            // TODO: 将大型字段设置为 null。
            Disposed = true;
        }

        //TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~ServerManagerBase()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }
    }
}
