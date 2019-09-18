using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.DatabaseQuery
{
    public class PmatchQuery
    {
        public static List<Dictionary<string, object>> GetProfileFromEmail(Dictionary<string, string> dict)
        {
            return GPSPServer.DB.Query("SELECT profiles.profileid, nick, uniquenick, lastname, firstname, email, namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE users.email = @P0 GROUP BY nick", dict["email"]);
        }
        public static List<Dictionary<string, object>> PlayerMatch(Dictionary<string, string> dict)
        {
            return GPSPServer.DB.Query("SELECT profiles.nick,profiles.statuscode,profiles.statusstring FROM profiles " +
                "INNER  JOIN namespace ON namespace.profileid = profiles.profileid " +
                "WHERE namespace.productid = @P0 AND profiles.profileid = @P1 ",
                dict["productID"], dict["profileid"]);
        }

    }
}
