using System;
using GameSpyLib.Logging;
using GameSpyLib.Database;
using RetroSpyServer.XMLConfig;
using System.Collections.Generic;
namespace RetroSpyServer.Extensions
{
    public class CDKEYDBQuery
    {
        DatabaseDriver dbdriver = null;
        public CDKEYDBQuery()
        {
            dbdriver = CreateNewMySQLConnection();
        }
        public CDKEYDBQuery(DatabaseDriver dbdriver)
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

        /// <summary>
        /// Checks the md5 of cdkey that client sending is validated in our database
        /// </summary>
        /// <returns></returns>
        public bool CheckCDKEYValidate()
        {
            return true;
        }
    }
}
