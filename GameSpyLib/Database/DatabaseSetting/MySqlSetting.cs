using LinqToDB.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSpyLib.Database.DatabaseSetting
{
    public class MySqlSetting: ILinqToDBSettings
    {
        public MySqlSetting(string connString)
        {
            ConnString = connString;
        }

        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "MySql.Data.MySqlClient";
        public string DefaultDataProvider => "MySql.Data.MySqlClient";

        public string ConnString { get; protected set; }

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSetting
                    {
                        Name = "RetroSpy",
                        ProviderName = "MySql.Data.MySqlClient",
                        ConnectionString = ConnString
                    };
            }
        }
    }
}
