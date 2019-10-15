using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler.OthersList
{
    public class OthersListQuery
    {
        public static List<Dictionary<string, object>> GetOtherBuddyList(Dictionary<string, string> dict, string pid)
        {
            var result = GPSPServer.DB.Query("SELECT profileid,uniquenick FROM namespace WHERE profileid = @P0 AND namespaceid =@P1 AND gamename=@P2 ", pid, dict["namespaceid"], dict["gamename"]);

           return (result.Count == 0) ? null : result;
        }

    }
}
