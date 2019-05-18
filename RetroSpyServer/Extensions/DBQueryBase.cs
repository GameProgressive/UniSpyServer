using System;
using GameSpyLib.Logging;
using GameSpyLib.Database;
using RetroSpyServer.XMLConfig;
using System.Collections.Generic;

namespace RetroSpyServer.Extensions
{
    public class DBQueryBase
    {

        public DatabaseDriver dbdriver { get; protected set; }
        public DBQueryBase()
        {
            dbdriver = CreateNewMySQLConnection();
        }
        public DBQueryBase(DatabaseDriver dbdriver)
        {
            if (dbdriver == null)
                this.dbdriver = CreateNewMySQLConnection();//this is Mysql method
            else
                this.dbdriver = dbdriver;//this is SQLite method
        }

        /// <summary>
        /// This function creates a new MySQL database connection
        /// </summary>
        /// <returns>An instance of the connection or null if the connection could not be created</returns>
        public MySqlDatabaseDriver CreateNewMySQLConnection()
        {
            DatabaseConfiguration cfg = ConfigManager.xmlConfiguration.Database;

            MySqlDatabaseDriver driver = new MySqlDatabaseDriver(string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4}", cfg.Hostname, cfg.Databasename, cfg.Username, cfg.Password, cfg.Port));

            try
            {
                driver.Connect();
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Fatal);
                throw ex; // Without database the server cannot start
            }

            return driver;
        }
        public List<Dictionary<string, object>> Query(string Sql, params object[] Items)
        {
            return dbdriver.Query(Sql,Items);
        }
    }
}
