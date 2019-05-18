using System;
using GameSpyLib.Logging;
using GameSpyLib.Database;
using RetroSpyServer.XMLConfig;
using System.Collections.Generic;
namespace RetroSpyServer.Extensions
{
    public class CDKEYDBQuery:DBQueryBase
    {
        public CDKEYDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }

        public bool CheckCDKEYValidate()
        {
            return true;
        }
    }
}
