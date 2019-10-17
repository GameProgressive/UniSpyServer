using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Login
{
    //login request will have different key,value pairs
    //some have dict["uniquenick"], dict["productid"], dict["partnerid"], dict["gamename"]
    //some have firewall,port
    //some have partnerid,namespaceid,port,productid,gamename,sdkrevision
    public class LoginQuery
    {

        public static Dictionary<string, object> GetUserFromUniqueNick(Dictionary<string, string> dict)
        {
            var result = GPCMServer.DB.Query(
                @"SELECT profiles.profileid, profiles.nick, profiles.firstname, profiles.lastname, profiles.publicmask, profiles.latitude,
                 profiles.longitude,profiles.aim, profiles.picture, profiles.occupationid, profiles.incomeid, profiles.industryid,
                 profiles.marriedid, profiles.childcount, profiles.interests1,profiles.ownership1, profiles.connectiontype, profiles.sex, 
                 profiles.zipcode, profiles.countrycode, profiles.homepage, profiles.birthday, profiles.birthmonth ,profiles.birthyear, 
                 profiles.location, profiles.icquin, profiles.status,profiles.statstring, users.email, users.password, users.userstatus 
                 FROM profiles INNER JOIN users ON profiles.userid = users.userid INNER JOIN namespace ON profiles.profileid = namespace.profileid  
                 WHERE namespace.uniquenick = @P0 AND namespace.namespaceid = @P1"
                 , dict["uniquenick"], dict["namespaceid"]
                 );
            return (result.Count == 0) ? null : result[0];
        }

        public static Dictionary<string, object> GetUserFromNickAndEmail(Dictionary<string, string> dict)
        {
            var result = GPCMServer.DB.Query(@"SELECT profiles.profileid, profiles.firstname, profiles.lastname, profiles.publicmask, profiles.latitude,profiles.longitude,"
            + @"profiles.aim, profiles.picture, profiles.occupationid, profiles.incomeid, profiles.industryid,profiles.marriedid, profiles.childcount, "
            + @"profiles.interests1,profiles.ownership1, profiles.connectiontype, profiles.sex,profiles.zipcode, profiles.countrycode, profiles.homepage, "
            + @"profiles.birthday, profiles.birthmonth ,profiles.birthyear,profiles.location, profiles.icquin,profiles.status,profiles.statstring, users.password, users.userstatus, namespace.uniquenick"
            + @" FROM profiles INNER JOIN users ON profiles.userid = users.userid INNER JOIN namespace ON profiles.profileid = namespace.profileid "
            + @"WHERE  namespace.namespaceid = @P0  AND profiles.nick = @P1 AND  users.email=@P2", dict["namespaceid"], dict["nick"], dict["email"]);
            return (result.Count == 0) ? null : result[0];
        }

        /// <summary>
        /// update sessionkey in database, when we are going to get someone's profile
        /// we first find uniquenick by search profileid and namespaceid and productid and partnerid  
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sesskey"></param>
        /// <param name="player"></param>
        public static void UpdateSessionKey(Dictionary<string, object> dict, uint namespaceid, ushort sesskey, Guid guid)
        {
            Dictionary<string, object> temp = GPCMServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND namespaceid = @P1 ", dict["profileid"], namespaceid)[0];
            uint id = Convert.ToUInt32(temp["id"]);
            GPCMServer.DB.Execute("UPDATE namespace SET guid = @P0 WHERE id = @P1 ", guid.ToString(), id);
            GPCMServer.DB.Execute("UPDATE namespace SET sesskey = @P0 WHERE id = @P1 ", sesskey, id);
        }


        public static void UpdateStatus(long lastOnlineTime, string ip, uint profileId, uint status)
        {
            GPCMServer.DB.Execute("UPDATE profiles SET status=@P0 WHERE profileid=@P1 ", status, profileId);
            uint userid = Convert.ToUInt32(GPCMServer.DB.Query("SELECT userid FROM profiles WHERE profileid= @P0", profileId)[0]);
            GPCMServer.DB.Execute("UPDATE users SET lastip=@P0, lastonline=@P1 WHERE userid = @P2", ip, lastOnlineTime, userid);
        }
        public static void UpdateSessionKeyAndGuid(uint profileid, uint namespaceid, uint sessionkey, Guid guid)
        {
            GPCMServer.DB.Execute("UPDATE namespace SET sesskey=@P0, guid =@P1 WHERE profileid =@P2 AND namespaceid = @P3", sessionkey, guid.ToString(), profileid, namespaceid);
        }

        public static void ResetAllStatusAndSessionKey()
        {
            GPCMServer.DB.Execute("UPDATE profiles SET status=@P0,statstring =@P1,location = @P2",GPEnum.Offline,"","");
            //GPCMServer.DB.Execute("UPDATE namespace SET sesskey = NULL");
            GPCMServer.DB.Execute("UPDATE namespace SET guid = NULL");
        }
    }
}
