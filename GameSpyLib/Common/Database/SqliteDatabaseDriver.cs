using System.Data.SQLite;
using System.Data.Common;

namespace GameSpyLib.Database
{
    /// <summary>
    /// create a sqlite databasedriver that can access to sqlite database
    /// </summary>
    public class SqliteDatabaseDriver : DatabaseDriver
    {
        public SqliteDatabaseDriver(string ConnectionString)
        {
            Connection = new SQLiteConnection(ConnectionString);
            DatabaseEngine = DatabaseEngine.Sqlite;
        }

        public override DbCommand CreateCommand(string QueryString)
        {
            return new SQLiteCommand(QueryString, Connection as SQLiteConnection);
        }

        public override DbParameter CreateParam()
        {
            return (new SQLiteParameter() as DbParameter);
        }
    }
}
