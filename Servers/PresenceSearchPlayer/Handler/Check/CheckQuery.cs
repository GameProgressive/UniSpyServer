using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Check
{
    class CheckQuery
    {
        public static Dictionary<string,object> GetProfileidFromNickEmailPassword(string email,string passenc,string nick)
        {           
            var result = GPSPServer.DB.Query("SELECT profileid FROM profiles " +
                " INNER JOIN users ON users.userid=profiles.userid " +
                " WHERE users.email=@P0 AND " +
                "users.password = @P1 AND " +
                "profiles.nick = @P2", email,passenc, nick);
            return (result.Count == 0) ? null : result[0];
        }

    }
}
