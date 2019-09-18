using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.DatabaseQuery
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
            //uniquenick existed 
            //if (Query("SELECT profileid FROM profiles WHERE uniquenick=@P0", uniquenick).Count > 0)
            bool isUniquenickExist = GPSPServer.DB.Query("SELECT namespace.profileid FROM namespace,users,profiles WHERE " +
                    "namespace.uniquenick = @P0 AND namespace.namespaceid = @P1 " +
                    "AND namespace.partnerid =@P2 AND namespace.productid=@P3 " +
                    "AND users.email = @P4 AND profiles.nick = @P5", dict["uniquenick"], dict["namespaceid"], dict["partnerid"], dict["productID"], dict["email"], dict["nick"]).Count > 0;

            if (isUniquenickExist)
                return true;
            else
                return false;
        }


        public static uint GetUseridFromEmail(Dictionary<string, string> dict)
        {
            uint userid;
            List<Dictionary<string, object>> temp = GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0", dict["email"]);
            if (temp.Count > 0)
            {
                userid = (uint)temp[0]["userid"];
                return userid;
            }
            else
            {
                //ToDo Finish this add account in database and return a userid
                GPSPServer.DB.Execute("INSERT INTO users(email,password,userstatus) VALUES (@P0, @P1, 1)", dict["email"], dict["passenc"]);

                userid = (uint)GPSPServer.DB.Query("SELECT userid FROM users WHERE email=@P0", dict["email"])[0]["userid"];

                return userid;
            }
        }


        public static Dictionary<string,object> CreateUserWithNick(Dictionary<string, string> dict, uint userid)
        {
            //we check is there exit a profile
            var result = GPSPServer.DB.Query("SELECT profiles.profileid FROM profiles INNER JOIN users ON profiles.userid = users.userid WHERE users.userid = @P0 AND profiles.nick=@P1", userid, dict["nick"]);

            uint pid = Convert.ToUInt32(result[0]["profileid"]);
            //if the profile is not exist we create one and update the information on namespaceid     
            if (result == null)
            {                
                //create profile
                GPSPServer.DB.Execute("INSERT INTO profiles(userid,nick) VALUES (@P0,@P1)", userid, dict["nick"]);
                //get the profileid
                var resultList = GPSPServer.DB.Query("SELECT profileid FROM profiles INNER JOIN users WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.userid = @P1", dict["nick"], userid);
                //update data in namespace
                //if (dict.ContainsKey("productID")&&!dict.ContainsKey("gamename")&&!dict.ContainsKey("partnerid")&&!dict.ContainsKey("namespaceid"))
                //{
                //    GPSPServer.DB.Execute("INSERT INTO namespace(profileid,uniquenick,productid VALUES (@P0,@P1,@P2)", pid, dict["uniquenick"], dict["productID"]);
                //}
                //else
                GPSPServer.DB.Execute("INSERT INTO namespace(profileid,namespaceid,uniquenick,partnerid,productid,gamename VALUES (@P0,@P1,@P2,@P3,@P4,@P5)", pid, dict["namespaceid"], dict["uniquenick"], dict["partnerid"], dict["productID"], dict["gamename"]);
                //return profileid.
                return resultList[0];                
            }
            else
            //if profileid exist we check namespaceid gamename partnerid on namespace table and create information on namespace table
            {
                // we check if the information in namespace is exist
                var resultList = GPSPServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND namespaceid = @P1 AND partnerid = @P2 AND productid = @P3 AND uniquenick = @P4 ", pid, dict["namespaceid"], dict["partnerid"], dict["productID"], dict["uniquenick"]);
                //if the information is exist in namespace we return 0
                if (resultList.Count==0)
                {
                    //added the information to namespace table;
                    GPSPServer.DB.Execute("INSERT INTO namespace(profileid,namespaceid,uniquenick,partnerid,productid,gamename) VALUES (@P0,@P1,@P2,@P3,@P4,@P5)", pid, dict["namespaceid"], dict["uniquenick"], dict["partnerid"], dict["productID"], dict["gamename"]);
                    return result[0];
                }
                else
                {
                   
                    return null;
                }
            }
        }

    }
}
