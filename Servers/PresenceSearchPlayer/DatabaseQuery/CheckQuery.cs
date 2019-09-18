using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.DatabaseQuery
{
    class CheckQuery
    {
        public static List<Dictionary<string, object>> GetProfileidFromNickEmailPassword(Dictionary<string, string> dict)
        {
            return GPSPServer.DB.Query("SELECT profileid FROM profiles " +
                " INNER JOIN users ON users.userid=profiles.userid " +
                " WHERE users.email=@P0 AND " +
                "users.password = @P1 AND " +
                "profiles.nick = @P2", dict["email"], dict["passenc"], dict["nick"]);
        }
    }
}
