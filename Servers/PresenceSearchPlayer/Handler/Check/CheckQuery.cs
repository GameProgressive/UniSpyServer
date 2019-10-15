using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Check
{
    class CheckQuery
    {
        public static int GetProfileidFromNickEmailPassword(Dictionary<string, string> dict)
        {
            //if (dict.ContainsKey("email") && dict.ContainsKey("nick") && dict.ContainsKey("passenc"))
            var result = GPSPServer.DB.Query("SELECT profileid FROM profiles " +
                " INNER JOIN users ON users.userid=profiles.userid " +
                " WHERE users.email=@P0 AND " +
                "users.password = @P1 AND " +
                "profiles.nick = @P2", dict["email"], dict["passenc"], dict["nick"]);
            return (result.Count == 0) ? -1 : Convert.ToInt32(result[0]["profileid"]);
        }

    }
}
