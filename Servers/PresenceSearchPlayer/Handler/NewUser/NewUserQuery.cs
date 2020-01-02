using System.Collections.Generic;
using System;

namespace PresenceSearchPlayer.Handler.NewUser
{
    public class NewUserQuery
    {
        public static void UpdateOtherInfo(string uniquenick,uint namespaceid, Dictionary<string, string> dict)
        {
            uint id = Convert.ToUInt32(GPSPServer.DB.Query("SELECT id FROM namespace WHERE uniquenick = @P0 AND namespaceid = @P1", uniquenick, namespaceid)[0]["id"]);

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
            if (result.Count == 0)
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

        public static void FindOrCreateProfileOnProfileTable(string nick, uint userid, out uint profileid)
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
            var result = GPSPServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND namespaceid = @P1 AND uniquenick=@P2", profileid, namespaceid,uniquenick);
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
