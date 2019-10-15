using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler.Others
{
    public class OthersQuery
    {
        public static List<Dictionary<string, object>> GetOtherBuddy(Dictionary<string, string> dict)
        {
           var result =  GPSPServer.DB.Query("SELECT profiles.nick,profiles.firstname,profiles.lastname,namespace.uniquenick, users.email " +
                "FROM profiles inner join namespace on namespace.profileid=profiles.profileid " +
                "INNER JOIN users ON users.userid = profiles.userid  " +
                "WHERE namespace.profileid = @P0 AND namespace.namespaceid=@P1",
                dict["profileid"], dict["namespaceid"]);
            return (result.Count == 0) ? null : result;
        }
    }
}
