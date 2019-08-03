using GameSpyLib.Database;
using System.Collections.Generic;

namespace RetroSpyServer.DBQueries
{
    public class GPSPDBQuery : DBQueryBase
    {

        public GPSPDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }

        public bool IsEmailValid(string email)
        {
            return Query("SELECT userid FROM users WHERE `email`=@P0", email).Count > 0;
        }

        public List<Dictionary<string, object>> RetriveNicknames(string email, string password)
        {
            return Query("SELECT profiles.nick, profiles.uniquenick FROM profiles " +
            "INNER JOIN users ON users.userid=profiles.userid " +
            "WHERE LOWER(users.email)=@P0 AND LOWER(users.password)=@P1" ,
            email.ToLowerInvariant(), password.ToLowerInvariant());

        }
        /// <summary>
        /// If a nick is exist in database return false
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public bool IsUniqueNickExist(string uniquenick)
        {
            //uniquenick existed 
            if (Query("SELECT profileid FROM profiles WHERE uniquenick=@P0", uniquenick).Count > 0)
                return false;
            else
                return true;
        }
    }
}
