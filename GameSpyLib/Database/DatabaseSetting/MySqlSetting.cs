using LinqToDB.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSpyLib.Database.DatabaseSetting
{
    class MySqlSetting: ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "MySql.Data.MySqlClient";
        public string DefaultDataProvider => "MySql.Data.MySqlClient";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSetting
                    {
                        Name = "RetroSpy",
                        ProviderName = "MySql.Data.MySqlClient",
                        ConnectionString = @"Server=localhost;Port=3306;Database=retrospy;Uid=root;Pwd=0000"
                    };
            }
        }
    }
}
