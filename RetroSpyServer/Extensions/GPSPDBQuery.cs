using System;
using GameSpyLib.Logging;
using GameSpyLib.Database;
using RetroSpyServer.XMLConfig;
using System.Collections.Generic;

namespace RetroSpyServer.Extensions
{
    public class GPSPDBQuery:DBQueryBase
    {
        public GPSPDBQuery() : base()
        {
        }
        public GPSPDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }

        public bool IsEmailValid(string Email)
        {
            if (dbdriver.Query("SELECT profileid FROM profiles WHERE `email`=@P0", Email).Count == 0)
                return true;
            else
                return false;

        }
        public List<Dictionary<string, object>> RetriveNicknames(string email, string password)
        {
            return dbdriver.Query("SELECT profiles.nick, profiles.uniquenick FROM profiles INNER JOIN users ON profiles.userid=users.userid WHERE LOWER(users.email)=@P0 AND LOWER(users.password)=@P1", email.ToLowerInvariant(), password.ToLowerInvariant());
        }
    }
}