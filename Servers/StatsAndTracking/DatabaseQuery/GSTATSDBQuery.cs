using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Database;

namespace StatsAndTracking
{
    public class GSTATSDBQuery : DBQueryBase
    {
        public GSTATSDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }
    }
}
