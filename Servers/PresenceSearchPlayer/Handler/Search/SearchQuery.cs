using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Search
{
    public class SearchQuery
    {
        public static List<Dictionary<string, object>> GetProfileFromNickEmail(string nick,string email,uint namespaceid)
        {
            List<Dictionary<string, object>> queryResult = GPSPServer.DB.Query("SELECT profiles.profileid,profiles.nick,namespace.uniquenick,profiles.lastname,profiles.firstname,users.email,namespace.namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE users.email = @P0 AND profiles.nick = @P1 AND namespace.namespaceid = @P2", email, nick,namespaceid);
            return (queryResult.Count == 0) ? null : queryResult;
        }

        public static List<Dictionary<string, object>> GetProfileFromNick(string nick, uint namespaceid)
        {
            List<Dictionary<string, object>> queryResult = GPSPServer.DB.Query("SELECT profiles.profileid,profiles.nick,namespace.uniquenick,profiles.lastname,profiles.firstname,users.email,namespace.namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE profiles.nick = @P0 AND namespace.namespaceid = @P1", nick, namespaceid);
            return (queryResult.Count == 0) ? null : queryResult;
        }

        /// <summary>
        /// get user profile by uniquenick and namespaceid
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetProfileFromUniquenick(string uniquenick,uint namespaceid)
        {
            List<Dictionary<string, object>> result = GPSPServer.DB.Query(@"SELECT profiles.profileid,profiles.nick,namespace.uniquenick,profiles.lastname,profiles.firstname,users.email,namespace.namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE namespace.uniquenick=@P0 AND namespace.namespaceid = @P1", uniquenick, namespaceid);
            return (result.Count == 0) ? null : result;
        }

        public static List<Dictionary<string, object>> GetProfileFromEmail(Dictionary<string, string> dict)
        {
            var result = GPSPServer.DB.Query(
                "SELECT profiles.profileid, profiles.nick, namespace.uniquenick, profiles.lastname, profiles.firstname, users.email, namespace.namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE users.email = @P0 GROUP BY nick", dict["email"]);
            return (result.Count == 0) ? null : result;
        }

    }
}
