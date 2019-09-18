using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.DatabaseQuery
{
    public class UniqueSearchQuery
    {
        public static bool IsUniqueNickExist(Dictionary<string, string> dict)
        {
            var result = GPSPServer.DB.Query("SELECT uniquenick FROM namespace " +
                 "WHERE uniquenick=@P0 AND namespaceid=@P1 AND gamename = @P2",
                dict["preferrednick"], dict["namespaceid"], dict["gamename"]);

            if (result==null)
                return true;
            else
                return false;
        }
    }
}
