using GameSpyLib.Database;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer
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

        public List<Dictionary<string, object>> RetriveNicknames(Dictionary<string,string> dict)
        {
            return Query("SELECT profiles.nick, namespace.uniquenick FROM profiles,namespace,users WHERE users.email=@P0 AND users.password=@P1 GROUP BY nick", dict["email"], dict["passenc"]);
           
        }

        public List<Dictionary<string, object>> GetProfileFromProfileid(Dictionary<string,string> dict)
        {
            return Query("SELECT nick,uniquenick,lastname,firstname,email,namespaceid FROM profiles,users,namespace WHERE profiles.profileid=@P0", dict["profileid"]);
       }

        /// <summary>
        /// If a nick is exist in database return userid, if not exist creating one and return userid.
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public bool IsUniqueNickExist(string uniquenick)
        {
            //uniquenick existed 
            if (Query("SELECT profileid FROM profiles WHERE uniquenick=@P0", uniquenick).Count > 0)
                return true;
            else
                return false;
        }        
        public uint GetuseridFromEmail(Dictionary<string,string>dict)
        {
            uint userid;
            List<Dictionary<string, object>> temp = Query("SELECT userid FROM users WHERE email=@P0", dict["email"]);
            if (temp.Count > 0)
            {
                userid = (uint)temp[0]["userid"];
                return userid;
            }
            else
            {
                //ToDo Finish this add account in database and return a userid
                Execute("INSERT INTO users(email,password,userstatus) VALUES (@P0, @P1, 1)", dict["email"], dict["passenc"]);

                userid =(uint) Query("SELECT userid FROM users WHERE email=@P0", dict["email"])[0]["userid"];
                
                return userid;
            }
        }

        public List<Dictionary<string,object>> GetProfileFromUniquenick(Dictionary<string, string> dict)
        {
           return Query("SELECT profiles.profileid,nick,uniquenick,lastname,firstname,email,namespaceid FROM profiles,users,namespace WHERE namespace.uniquenick = @P0 AND namespace.namespaceid = @P1", dict["uniquenick"], dict["namespaceid"]);
        }

        public uint GetprofileidFromEmail(string email)
        {
            uint profileid;
            List<Dictionary<string, object>> temp = Query("SELECT profileid FROM profiles " +
                "INNER JOIN users ON users.userid=profiles.userid " +
                "WHERE LOWER(users.email)=@P0", email);
            if (temp.Count > 0)
            {
                profileid = (uint)temp[0]["profileid"];
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
        public uint CreateUserWithNickOrUniquenick(Dictionary<string,string> dict,uint userid)
        {
            if (dict["uniquenick"] == "")
            {
                Execute("INSERT INTO profiles(userid,nick) VALUES (@P0,@P1,)", userid, dict["nick"]);
                uint profileid = (uint)Query("SELECT profileid FROM profiles INNER JOIN users WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.userid = @P1", dict["nick"], userid)[0]["profileid"];
                return profileid;

            }
            else
            {
                Execute("INSERT INTO profiles(userid,uniquenick,nick) VALUES (@P0,@P1,@P2)", userid, dict["uniquenick"], dict["nick"]);
                uint profileid = (uint)Query("SELECT profileid FROM profiles INNER JOIN users WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.uniquenick=@P1 AND profiles.userid = @P2", dict["nick"], dict["uniquenick"], userid)[0]["profileid"];
                return profileid;
            }
            //dict["nick"], dict["uniquenick"], dict["userid"], dict["email"],
            //        dict["passenc"], dict["productID"], dict["namespaceid"], dict["partnerid"], dict["gamename"]    
            
        }
    }
}
