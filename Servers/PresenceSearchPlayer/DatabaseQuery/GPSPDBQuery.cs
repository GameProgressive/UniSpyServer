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

        public  List<Dictionary<string, object>> GetProfileidFromNickEmailPassword(Dictionary<string, string> dict)
        {
            return Query("SELECT profileid FROM profiles " +
                " INNER JOIN users ON users.userid=profiles.userid " +
                " WHERE users.email=@P0 AND " +
                "users.password = @P1 AND " +
                "profiles.nick = @P2", dict["email"], dict["passenc"], dict["nick"]);
        }

        public List<Dictionary<string, object>> GetOtherBuddyList(Dictionary<string, string> dict, string pid)
        {
            throw new NotImplementedException();
        }

        internal List<Dictionary<string,object>> GetOtherBuddy(Dictionary<string, string> dict)
        {
            return Query("SELECT profiles.nick,firstname,lastname,uniquenick " +
                "FROM profiles inner join namespace on namespace.profileid=profiles.profileid " +
                "INNER JOIN users ON users.userid = profiles.userid  " +
                "WHERE namespace.profileid = @P0 AND namespace.namespaceid=@P1",
                dict["profileid"],dict["namespaceid"]);
        }

        public List<Dictionary<string, object>> RetriveNicknames(Dictionary<string, string> dict)
        {
            return Query("SELECT profiles.nick, namespace.uniquenick FROM profiles,namespace,users WHERE users.email=@P0 AND users.password=@P1 GROUP BY nick", dict["email"], dict["passenc"]);

        }

        public List<Dictionary<string, object>> GetProfileFromProfileid(Dictionary<string, string> dict)
        {
            return Query("SELECT nick,uniquenick,lastname,firstname,email,namespaceid FROM profiles,users,namespace WHERE profiles.profileid=@P0", dict["profileid"]);
        }

        /// <summary>
        /// If a nick is exist in database return userid, if not exist creating one and return userid.
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public bool IsUniqueNickExistForNewUser(Dictionary<string, string> dict)
        {
            //uniquenick existed 
            //if (Query("SELECT profileid FROM profiles WHERE uniquenick=@P0", uniquenick).Count > 0)
            bool isUniquenickExist = Query("SELECT namespace.profileid FROM namespace,users,profiles WHERE " +
                    "namespace.uniquenick = @P0 AND namespace.namespaceid = @P1 " +
                    "AND namespace.partnerid =@P2 AND namespace.productid=@P3 " +
                    "AND users.email = @P4 AND profiles.nick = @P5", dict["uniquenick"], dict["namespaceid"], dict["partnerid"], dict["productID"], dict["email"], dict["nick"]).Count > 0;

            if (isUniquenickExist)
                return true;
            else
                return false;
        }
        public bool IsUniqueNickExistForSuggest(Dictionary<string, string> dict)
        {
            bool isUniquenickExist = Query("SELECT uniquenick FROM namespace " +
                 "WHERE uniquenick=@P0 AND namespaceid=@P1 AND gamename = @P2", 
                dict["preferrednick"], dict["namespaceid"], dict["gamename"]).Count > 0;

            if (isUniquenickExist)
                return true;
            else
                return false;
        }
        public uint GetuseridFromEmail(Dictionary<string, string> dict)
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

                userid = (uint)Query("SELECT userid FROM users WHERE email=@P0", dict["email"])[0]["userid"];

                return userid;
            }
        }

        internal List<Dictionary<string, object>> GetProfileFromNickEmail(Dictionary<string, string> dict)
        {
            return Query("SELECT profiles.profileid,nick,uniquenick,lastname,firstname,email,namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE users.email = @P0 AND profiles.nick = @P1", dict["email"], dict["nick"]);
        }

        /// <summary>
        /// get user profile by uniquenick and namespaceid
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetProfileFromUniquenick(Dictionary<string, string> dict)
        {
            return Query(@"SELECT profiles.profileid,nick,uniquenick,lastname,firstname,email,namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE namespace.uniquenick=@P0 AND namespace.namespaceid = @P1", dict["uniquenick"], dict["namespaceid"]);
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

        public uint CreateUserWithNick(Dictionary<string, string> dict, uint userid)
        {
            //this may have problems
            bool isProfileExist = Query("SELECT profileid FROM users WHERE userid = @P0 AND nick=@P1",userid,dict["nick"]).Count>0;
            if (isProfileExist)
            {
                return 0;
            }
            else
            {
                Execute("INSERT INTO profiles(userid,nick) VALUES (@P0,@P1)", userid, dict["nick"]);
                uint profileid = (uint)Query("SELECT profileid FROM profiles INNER JOIN users WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.userid = @P1", dict["nick"], userid)[0]["profileid"];
                return profileid;
            }            
        }
        public uint CreateUserWithUnique(Dictionary<string, string> dict, uint userid)
        {
            Execute("INSERT INTO profiles(userid,nick) VALUES (@P0,@P1)", userid, dict["nick"]);
            uint profileid = (uint)Query("SELECT profileid FROM profiles INNER JOIN users WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.userid = @P1", dict["nick"], userid)[0]["profileid"];
            Execute("INSERT INTO namespace(profileid,uniquenick,namespaceid,partnerid,productid) VALUES (@P0,@P1,@P2,@P3,@P4)", profileid, dict["uniquenick"], dict["namespaceid"], dict["partnerid"], dict["productID"]);
            return profileid;
        }

        public List<Dictionary<string, object>> GetProfileFromEmail(Dictionary<string, string> dict)
        {
            return Query("SELECT profiles.profileid, nick, uniquenick, lastname, firstname, email, namespaceid FROM profiles INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE users.email = @P0 GROUP BY nick", dict["email"]);
        }

        public List<Dictionary<string, object>> PlayerMatch(Dictionary<string, string> dict)
        {
            return Query("SELECT profiles.nick,profiles.statuscode,profiles.statusstring FROM profiles " +
                "INNER  JOIN namespace ON namespace.profileid = profiles.profileid " +
                "WHERE namespace.productid = @P0 AND profiles.profileid = @P1 ",
                dict["productID"], dict["profileid"]);
        }
    }
}
