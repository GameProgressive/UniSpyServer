using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Nick
{
    /// <summary>
    /// get all nickname under a namespace
    /// </summary>
    public class NickQuery
    {
        public static List<Dictionary<string, object>> RetriveNicknames(string email, string password, uint namespaceid)
        {
            var result = GPSPServer.DB.Query(
                @"SELECT profiles.nick, namespace.uniquenick 
                FROM profiles 
                INNER JOIN namespace ON profiles.profileid = namespace.profileid 
                INNER  JOIN users ON users.userid = profiles.userid
                WHERE users.email = @P0 AND users.password = @P1 AND namespace.namespaceid = @P2",
                email, password, namespaceid);
            return (result.Count == 0) ? null : result;
        }
    }
}
