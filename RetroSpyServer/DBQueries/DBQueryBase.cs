using System;
using GameSpyLib.Logging;
using GameSpyLib.Database;
using RetroSpyServer.XMLConfig;
using System.Collections.Generic;

namespace RetroSpyServer.DBQueries
{
    public class DBQueryBase
    {
        //Create dbdriver so can be access from other class but just can set from this class
        public DatabaseDriver dbdriver { get; protected set; }
               
        /// <summary>
        /// If there is DatabaseDriver argument are passed to the constructor,
        /// decide whether use Mysql or Sqlite method.
        /// </summary>
        /// <param name="dbdriver">DatabaseDriver argument</param>
        public DBQueryBase(DatabaseDriver dbdriver)
        {
            if(dbdriver==null)
                this.dbdriver = CreateNewMySQLConnection();//this is mysql method
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
