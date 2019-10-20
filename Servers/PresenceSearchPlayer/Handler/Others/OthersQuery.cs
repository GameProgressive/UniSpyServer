using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.Others
{
    public class OthersQuery
    {
        public static List<Dictionary<string, object>> GetOtherBuddy(uint profileid,uint namespaceid)
        {
            var buddy = GPSPServer.DB.Query("SELECT friends.targetid FROM friends WHERE profileid = @P0 AND namespace = @P1", profileid, namespaceid)[0];
            uint[] buddyPIDs = buddy.Values.Cast<uint>().ToArray();

            List<Dictionary<string, object>> result= new List<Dictionary<string, object>>(buddyPIDs.Length);
            foreach (uint pid in buddyPIDs)
            {
               var info= GPSPServer.DB.Query(
                   @"SELECT profiles.nick,profiles.firstname,profiles.lastname, namespace.uniquenick, users.email 
                    FROM profiles INNER JOIN namespace ON namespace.profileid = profiles.profileid 
                    INNER JOIN users on users.userid = profiles.userid 
                    WHERE namespace.profileid= @P0 AND namespace.namespaceid=@P1",
                   pid,namespaceid)[0];
                result.Add(info);
            }
            return (result.Count == 0) ? null : result;
        }
    }
}
