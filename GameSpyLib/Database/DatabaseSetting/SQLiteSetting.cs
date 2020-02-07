using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace GameSpyLib.Database.DatabaseSetting
{
    public class SQLiteSetting: ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "SQLite";
        public string DefaultDataProvider => "SQLite";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSetting
                    {
                        Name = "RetroSpy",
                        ProviderName = "SQLite",
                        ConnectionString = @"Data Source=C:\Users\xiaojiuwo\Desktop\Linq2RetroSpy\bin\Debug\netcoreapp3.1\RetroSpy.sqlite"
                    };
            }
        }
    }
}
