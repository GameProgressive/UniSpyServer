using LinqToDB.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameSpyLib.Database.DatabaseSetting
{
    public class ConnectionStringSetting: IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }



}
