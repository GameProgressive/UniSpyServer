using MySql.Data.MySqlClient;
using System.Data.Common;

namespace GameSpyLib.Database.Entity
{
    public class MySqlDatabaseDriver : DatabaseDriver
    {
        public MySqlDatabaseDriver(string ConnectionString)
        {
            // Set class variables, and create a new connection builder
            Connection = new MySqlConnection(ConnectionString);
            DatabaseEngine = DatabaseEngine.Mysql;
        }

        public override DbCommand CreateCommand(string QueryString)
        {
            return new MySqlCommand(QueryString, Connection as MySqlConnection);
        }

        public override DbParameter CreateParam()
        {
            return (new MySqlParameter() as DbParameter);
        }
    }
}
