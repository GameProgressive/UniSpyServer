using System;
using System.Collections.Generic;
using System.Text;

namespace StatsAndTracking.Handler.AuthP
{
    public class AuthPQuery
    {
        public static Dictionary<string,object> SearchPlayerInfo()
        {
            return GStatsServer.DB.Query("SELECT * FROM profiles")[0];
        }
    }
}
