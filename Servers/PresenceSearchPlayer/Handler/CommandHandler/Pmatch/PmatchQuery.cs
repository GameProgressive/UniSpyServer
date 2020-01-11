using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler.Pmatch
{
    public class PmatchQuery
    {
        public static List<Dictionary<string, object>> GetProfileFromEmail(Dictionary<string, string> dict)
        {
            return GPSPServer.DB.Query("SELECT profiles.profileid, nick, uniquenick, lastname, firstname, email, namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE users.email = @P0 GROUP BY nick", dict["email"]);
        }
        public static List<Dictionary<string, object>> PlayerMatch(uint productid)
        {
            return GPSPServer.DB.Query(
                                                                @"SELECT profiles.profileid,profiles.nick,profiles.status,profiles.statstring 
                                                                FROM profiles 
                                                                INNER  JOIN namespace ON namespace.profileid = profiles.profileid 
                                                                WHERE namespace.productid = @P0 ",
                                                                productid
                                                                );
        }

    }
}
