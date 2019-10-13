﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.DatabaseQuery
{
    public class NickQuery
    {
        public static List<Dictionary<string, object>> RetriveNicknames(Dictionary<string, string> dict)
        {
            return GPSPServer.DB.Query("SELECT profiles.nick, namespace.uniquenick FROM profiles INNER JOIN namespace ON profiles.profileid = namespace.profileid INNER  JOIN users ON users.userid = profiles.userid WHERE users.email = @P0 AND users.password = @P1 AND namespace.namespaceid = @P2",dict["email"],dict["passenc"],dict["namespaceid"]);
            //return GPSPServer.DB.Query("SELECT profiles.nick, namespace.uniquenick FROM profiles,namespace,users WHERE users.email=@P0 AND users.password=@P1 GROUP BY nick", dict["email"], dict["passenc"]);

        }
    }
}
