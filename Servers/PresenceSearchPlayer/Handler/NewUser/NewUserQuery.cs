using System.Collections.Generic;
using System;

namespace PresenceSearchPlayer.Handler.NewUser
{
    public class NewUserQuery
    {
        /// <summary>
        /// If a nick is exist in database return userid, if not exist creating one and return userid.
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public static bool IsUniqueNickExistForNewUser(Dictionary<string, string> dict)
        {
            //will have different request keys.!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //uniquenick existed 
            //if (Query("SELECT profileid FROM profiles WHERE uniquenick=@P0", uniquenick).Count > 0)
            bool isUniquenickExist;
            if (dict.ContainsKey("uniquenick") && dict.ContainsKey("partnerid") && dict.ContainsKey("namespaceid") && dict.ContainsKey("productid"))
                isUniquenickExist = GPSPServer.DB.Query("SELECT namespace.profileid FROM namespace,users,profiles WHERE " +
                        "namespace.uniquenick = @P0 AND namespace.namespaceid = @P1 " +
                        "AND namespace.partnerid =@P2 AND namespace.productid=@P3 " +
                        "AND users.email = @P4 AND profiles.nick = @P5", dict["uniquenick"], dict["namespaceid"], dict["partnerid"], dict["productid"], dict["email"], dict["nick"]).Count > 0;
            else
            {
                isUniquenickExist = GPSPServer.DB.Query("SELECT namespace.profileid FROM namespace,users,profiles WHERE " +
                        "namespace.uniquenick = @P0 AND namespace.namespaceid = @P1 " +
                        " AND namespace.productid=@P2 " +
                        "AND users.email = @P3 AND profiles.nick = @P4", dict["uniquenick"], dict["namespaceid"], dict["productid"], dict["email"], dict["nick"]).Count > 0;
            }
            if (isUniquenickExist)
                return true;
            else
                return false;
        }

        //private static int GetUseridFromTableUsers(Dictionary<string, string> dict)
        //{
        //    int userid;
        //    var result = GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0", dict["email"]);
        //    if (result.Count > 0)
        //    {
        //        userid = Convert.ToInt32(temp[0]["userid"]);
        //        return userid;
        //    }
        //    else
        //    {
        //        return -1;
        //    }
        //}


        public static uint IsEmailAndPasswordExist(string email, string password)
        {
            var result = GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0 AND password=@P1", email, password)[0];
            if (result == null)
            {
                return 0;
            }
            return Convert.ToUInt32(result["userid"]);
        }


        /// <summary>
        /// If user is existed verify his email and password, otherwise create it
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static uint CreateUserOnTableUsers(Dictionary<string, string> dict)
        {
            uint userid;
            //ToDo Finish this add account in database and return a userid
            GPSPServer.DB.Execute("INSERT INTO users(email,password) VALUES (@P0, @P1)", dict["email"], dict["passenc"]);
            userid = Convert.ToUInt32(GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0", dict["email"])[0]["userid"]);
            return userid;
        }
        /// <summary>
        /// create account
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="userid"></param>
        /// <returns>if user existed we returns -1 otherwise return profileid</returns>
        public static uint CreateProfileOnTableProfiles(Dictionary<string, string> dict, int userid)
        {
            uint profileid;
            //we check is there exit a profile
            var result = GPSPServer.DB.Query("SELECT profiles.profileid FROM profiles INNER JOIN users ON profiles.userid = users.userid WHERE users.userid = @P0 AND profiles.nick=@P1", userid, dict["nick"]);

            if (result.Count == 0)
            {
                //create profile
                GPSPServer.DB.Execute("INSERT INTO profiles(userid,nick) VALUES (@P0,@P1)", userid, dict["nick"]);
                //get the profileid
                result = GPSPServer.DB.Query("SELECT profileid FROM profiles INNER JOIN users WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.userid = @P1", dict["nick"], userid);
                profileid = Convert.ToUInt32(result[0]["profileid"]);
            }
            else//if the profile is not exist we create one   
            {
                profileid = Convert.ToUInt32(result[0]["profileid"]);
            }
            //any way we return profileid;
            return profileid;
        }

        public static bool CreateSubprofileOnTableNamespace(Dictionary<string, string> dict, int profileid)
        {
            var result = GPSPServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND namespaceid = @P1", profileid, dict["namespaceid"]);
            if (result.Count == 0)
            {
                GPSPServer.DB.Execute("INSERT INTO namespace(profileid,namespaceid,uniquenick) VALUES (@P0,@P1,@P2)", profileid, dict["namespaceid"], dict["uniquenick"]);
                result = GPSPServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND namespaceid = @P1 AND uniquenick = @P2", profileid, dict["namespaceid"], dict["uniquenick"]);
                UpdateOtherInfo((uint)result[0]["id"], dict);
                //return profileid.
                return true;
            }
            else
            {
                //account exist creation failed
                return false;
            }

        }

        public static void UpdateOtherInfo(uint id, Dictionary<string, string> dict)
        {
            if (dict.ContainsKey("productid"))
            {
                GPSPServer.DB.Execute("UPDATE namespace SET productid = @P0 WHERE id = @P1", dict["productid"], id);
            }
            if (dict.ContainsKey("partnerid"))
            {
                GPSPServer.DB.Execute("UPDATE namespace SET partnerid = @P0 WHERE id = @P1", dict["partnerid"], id);
            }
            if (dict.ContainsKey("gamename"))
            {
                GPSPServer.DB.Execute("UPDATE namespace SET gamename = @P0 WHERE id = @P1", dict["gamename"], id);
            }
            if (dict.ContainsKey("firewall"))
            {
                GPSPServer.DB.Execute("UPDATE namespace SET firewall = @P0 WHERE id = @P1", dict["firewall"], id);
            }
            if (dict.ContainsKey("port"))
            {
                GPSPServer.DB.Execute("UPDATE namespace SET port = @P0 WHERE id = @P1", dict["port"], id);
            }
            if (dict.ContainsKey("cdkeyenc"))
            {
                GPSPServer.DB.Execute("UPDATE namepace SET cdkeyenc = @P0 WHERE id = @P1", dict["cdkeyenc"], id);
            }
        }

        public static bool IsAccountExist(string email)
        {
            var result = GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0", email)[0];
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public static bool IsAccountCorrect(string email, string passenc, out uint userid)
        {
            var result = GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0 AND password=@P1", email, passenc);
            if (result.Count == 0)
            {
                userid = 0;
                return false;
            }
            userid = Convert.ToUInt32(result[0]["userid"]);
            return true;
        }

        public static void CreateAccountOnUsersTable(string email, string passenc, out uint userid)
        {
            GPSPServer.DB.Execute("INSERT INTO users(email,password) VALUES (@P0, @P1)", email, passenc);
            userid = Convert.ToUInt32(GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0", email, passenc)[0]["userid"]);
        }

        public static void FindOrCreateProfieOnProfileTable(string nick, uint userid, out uint profileid)
        {
            //we check is there exit a profile
            var result = GPSPServer.DB.Query(
                "SELECT profiles.profileid " +
                "FROM profiles " +
                "INNER JOIN users ON profiles.userid = users.userid " +
                "WHERE users.userid = @P0 AND profiles.nick=@P1", userid, nick);
            //if the profile is not exist we create one
            if (result.Count == 0)
            {
                //create profile
                GPSPServer.DB.Execute("INSERT INTO profiles(userid,nick) VALUES (@P0,@P1)", userid, nick);
                //get the profileid
                profileid = Convert.ToUInt32(GPSPServer.DB.Query(
                    "SELECT profileid " +
                    "FROM profiles " +
                    "INNER JOIN users " +
                    "WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.userid = @P1"
                    , nick, userid)[0]["profileid"]);
            }
            else
            {
                profileid = Convert.ToUInt32(result[0]["profileid"]);
            }


        }

        public static bool FindOrCreateSubProfileOnNamespaceTable(string uniquenick, ushort namespaceid, uint profileid)
        {
            var result = GPSPServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND namespaceid = @P1", profileid, namespaceid);
            if (result.Count == 0)
            {
                GPSPServer.DB.Execute("INSERT INTO namespace(profileid,namespaceid,uniquenick) VALUES (@P0,@P1,@P2)", profileid, namespaceid, uniquenick);
                return true;
            }
            //account existed, creation failed
            return false;
        }




    }
}
