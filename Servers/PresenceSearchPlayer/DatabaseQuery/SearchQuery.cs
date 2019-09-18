using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.DatabaseQuery
{
    public class SearchQuery
    {
        public static List<Dictionary<string, object>> GetProfileFromNickEmail(Dictionary<string, string> dict)
        {
            var queryResult = GPSPServer.DB.Query("SELECT profiles.profileid,nick,uniquenick,lastname,firstname,email,namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE users.email = @P0 AND profiles.nick = @P1", dict["email"], dict["nick"]);
            return (queryResult.Count == 0) ? null : queryResult;
        }
        /// <summary>
        /// get user profile by uniquenick and namespaceid
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetProfileFromUniquenick(Dictionary<string, string> dict)
        {
            return GPSPServer.DB.Query(@"SELECT profiles.profileid,nick,uniquenick,lastname,firstname,email,namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE namespace.uniquenick=@P0 AND namespace.namespaceid = @P1", dict["uniquenick"], dict["namespaceid"]);
        }

        public static List<Dictionary<string, object>> GetProfileFromEmail(Dictionary<string, string> dict)
        {
            return GPSPServer.DB.Query("SELECT profiles.profileid, nick, uniquenick, lastname, firstname, email, namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE users.email = @P0 GROUP BY nick", dict["email"]);
        }

    }
}
