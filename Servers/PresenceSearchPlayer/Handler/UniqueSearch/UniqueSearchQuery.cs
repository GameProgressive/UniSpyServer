using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler.UniqueSearch
{
    public class UniqueSearchQuery
    {
        public static bool IsUniqueNickExist(Dictionary<string, string> dict)
        {
            var result = GPSPServer.DB.Query("SELECT uniquenick FROM namespace " +
                 "WHERE uniquenick=@P0 AND namespaceid=@P1",
                dict["preferrednick"], dict["namespaceid"]);

            if (result==null)
                return true;
            else
                return false;
        }
    }
}
