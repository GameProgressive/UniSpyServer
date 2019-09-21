using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.DatabaseQuery
{
    class CheckQuery
    {
        public static List<Dictionary<string, object>> GetProfileidFromNickEmailPassword(Dictionary<string, string> dict)
        {
            //if (dict.ContainsKey("email") && dict.ContainsKey("nick") && dict.ContainsKey("passenc"))
                return GPSPServer.DB.Query("SELECT profileid FROM profiles " +
                    " INNER JOIN users ON users.userid=profiles.userid " +
                    " WHERE users.email=@P0 AND " +
                    "users.password = @P1 AND " +
                    "profiles.nick = @P2", dict["email"], dict["passenc"], dict["nick"]);
            //else
            //{
            //    int userid = (int) GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0 ", dict["email"])[0]["userid"];
            //    return GPSPServer.DB.Query("SELECT profileid FROM namespace WHERE partnerid=@P0 AND gamename = @P1 ", dict["partnerid"],dict["gamename"]);
            //}
                
        }
    }
}
