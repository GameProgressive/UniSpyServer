using GameSpyLib.Database;
using System;
using System.Collections.Generic;

namespace RetroSpyServer.DBQueries
{
    public class GPSPDBQuery : DBQueryBase
    {

        public GPSPDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }
               
        /// <summary>
        /// If an email is existed in database, this will return TRUE
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsEmailValid(string email)
        {
            return Query("SELECT userid FROM users WHERE `email`=@P0", email).Count > 0;
        }

        public List<Dictionary<string, object>> RetriveNicknames(string email, string password)
        {
            
            return Query("SELECT profiles.nick, profiles.uniquenick FROM profiles " +
            "INNER JOIN users ON users.userid=profiles.userid " +
            "WHERE LOWER(users.email)=@P0 AND LOWER(users.password)=@P1",
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
        public int GetuseridFromEmail(string email)
        {
            int userid;
            List<Dictionary<string, object>> temp = Query("SELECT userid FROM users WHERE email=@P0", email);
            if (temp.Count > 0)
            {
                userid = (int)temp[0]["userid"];
                return userid;
            }
            else
            {
                //ToDo Finish this add account in database and return a userid
                userid = 0;
                return userid;
            }
        }
        public int GetprofileidFromEmail(string email)
        {
            int profileid;
            List<Dictionary<string, object>> temp = Query("SELECT profileid FROM profiles " +
                "INNER JOIN users ON users.userid=profiles.userid " +
                "WHERE LOWER(users.email)=@P0", email);
            if (temp.Count > 0)
            {
                profileid = (int)temp[0]["profileid"];
                return profileid;
            }
            else
            {
                //todo
                profileid = 1;
                return profileid;
                throw new NotImplementedException();
            }
        }
        public int CreateUserWithUniquenick(string nick,string userid)
        {
            //dict["nick"], dict["uniquenick"], dict["userid"], dict["email"],
            //        dict["passenc"], dict["productID"], dict["namespaceid"], dict["partnerid"], dict["gamename"]            
            int profileid = 1;
            return profileid;
            throw new NotImplementedException();
        }
        public int CreateUserWithNick(string nick,string uniquenick,string userid)
        {
            int profileid = 1;

            return profileid;
            throw new NotImplementedException();
        }


    }
}
