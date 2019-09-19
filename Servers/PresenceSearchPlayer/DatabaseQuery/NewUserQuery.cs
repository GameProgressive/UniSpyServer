using GameSpyLib.Logging;
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

        /// <summary>
        /// create account
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="userid"></param>
        /// <returns>if user existed we returns -1 otherwise return profileid</returns>
        public static int CreateUserWithNick(Dictionary<string, string> dict, uint userid)
        {
            //we check is there exit a profile
            var result0 = GPSPServer.DB.Query("SELECT profiles.profileid FROM profiles INNER JOIN users ON profiles.userid = users.userid WHERE users.userid = @P0 AND profiles.nick=@P1", userid, dict["nick"]);

            uint pid;
            //if the profile is not exist we create one and update the information on namespaceid     
            try
            {
                if (result0.Count==0)
                {

                    //create profile
                    GPSPServer.DB.Execute("INSERT INTO profiles(userid,nick) VALUES (@P0,@P1)", userid, dict["nick"]);
                    //get the profileid
                    var result1 = GPSPServer.DB.Query("SELECT profileid FROM profiles INNER JOIN users WHERE profiles.userid=users.userid AND profiles.nick = @P0 AND profiles.userid = @P1", dict["nick"], userid);
                    pid = Convert.ToUInt32(result1[0]["profileid"]);
                    //update data in namespace
                    //if (dict.ContainsKey("productID")&&!dict.ContainsKey("gamename")&&!dict.ContainsKey("partnerid")&&!dict.ContainsKey("namespaceid"))
                    //{
                    //    GPSPServer.DB.Execute("INSERT INTO namespace(profileid,uniquenick,productid VALUES (@P0,@P1,@P2)", pid, dict["uniquenick"], dict["productID"]);
                    //}
                    //else
                    GPSPServer.DB.Execute("INSERT INTO namespace(profileid,productid) VALUES (@P0,@P1)", pid, dict["productID"]);
                    var result2 = GPSPServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND productid = @P1", pid, dict["productID"]);
                    UpdateOtherInfo((uint)result2[0]["id"], dict);
                    //return profileid.
                    return (int)result1[0]["profileid"];
                }
                else
                //if profileid exist we check namespaceid gamename partnerid on namespace table and create information on namespace table
                {
                    pid = Convert.ToUInt32(result0[0]["profileid"]);
                    // we check if the information in namespace is exist
                    var result2 = GPSPServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND productid = @P1", pid, dict["productID"]);
                    //if the information is exist in namespace we return 0
                   
                    if (result2.Count == 0)
                    {
                        //added the information to namespace table;
                        //GPSPServer.DB.Execute("INSERT INTO namespace(profileid,namespaceid,uniquenick,partnerid,productid,gamename) VALUES (@P0,@P1,@P2,@P3,@P4,@P5)", pid, dict["namespaceid"], dict["uniquenick"], dict["partnerid"], dict["productID"], dict["gamename"]);
                        GPSPServer.DB.Execute("INSERT INTO namespace(profileid,productid) VALUES (@P0,@P1)", pid, dict["productID"]);
                        var result3= GPSPServer.DB.Query("SELECT id,profileid FROM namespace WHERE profileid = @P0 AND productid = @P1", pid, dict["productID"]);
                        UpdateOtherInfo((uint)result3[0]["id"], dict);
                        return (int)result3[0]["profileid"];
                    }
                    else
                    {
                        return -1;
                    }

                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
                return -1;
            }
        }
        public static void UpdateOtherInfo(uint id,Dictionary<string,string> dict)
        {
            if (dict.ContainsKey("uniquenick"))
            {
                GPSPServer.DB.Execute("UPDATE namespace(partnerid) VALUES (@P0) WHERE id = @P1", dict["uniquenick"], id);
            }
            if (dict.ContainsKey("partnerid"))
            {
                GPSPServer.DB.Execute("UPDATE namespace(partnerid) VALUES (@P0) WHERE id = @P1", dict["partnerid"],id);
            }
            if (dict.ContainsKey("namespaceid"))
            {
                GPSPServer.DB.Execute("UPDATE namespace(namespaceid) VALUES (@P0) WHERE id = @P1", dict["namespaceid"], id);
            }
            if (dict.ContainsKey("gamename"))
            {
                GPSPServer.DB.Execute("UPDATE namespace(gamename) VALUES (@P0) WHERE id = @P1", dict["gamename"], id);
            }
        }
    }
}
