using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.General.Login
{
    //login request will have different key,value pairs
    //some have dict["uniquenick"], dict["productid"], dict["partnerid"], dict["gamename"]
    //some have firewall,port
    //some have partnerid,namespaceid,port,productid,gamename,sdkrevision
    public class LoginQuery
    {

        public static List<Dictionary<string, object>> GetUserFromUniqueNick(string uniquenick, uint namespaceid)
        {
            var result = GPCMServer.DB.Query(
                @"SELECT profiles.profileid, namespace.uniquenick, profiles.nick, users.email, users.password, users.emailverified,users.banned 
                 FROM profiles INNER JOIN users ON profiles.userid = users.userid INNER JOIN namespace ON profiles.profileid = namespace.profileid  
                 WHERE namespace.uniquenick = @P0 AND namespace.namespaceid = @P1"
                 , uniquenick, namespaceid
                 );
            return (result.Count == 0) ? null : result;
        }

        public static List<Dictionary<string, object>> GetUserFromNickAndEmail(uint namespaceid, string nickname, string email)
        {
            var result = GPCMServer.DB.Query(@"SELECT profiles.profileid, namespace.uniquenick, users.password, users.emailverified, users.banned"
            + @" FROM profiles INNER JOIN users ON profiles.userid = users.userid INNER JOIN namespace ON profiles.profileid = namespace.profileid "
            + @"WHERE  namespace.namespaceid = @P0  AND profiles.nick = @P1 AND users.email=@P2", namespaceid, nickname, email);
            return (result.Count == 0) ? null : result;
        }

        /// <summary>
        /// update sessionkey in database, when we are going to get someone's profile
        /// we first find uniquenick by search profileid and namespaceid and productid and partnerid  
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sesskey"></param>
        /// <param name="player"></param>
        public static bool UpdateSessionKey(uint profileid, uint namespaceid, ushort sesskey, Guid guid)
        {
            List<Dictionary<string, object>> temp = GPCMServer.DB.Query("SELECT id FROM namespace WHERE profileid = @P0 AND namespaceid = @P1 ", profileid, namespaceid);

            if (temp.Count < 1)
                return false;

            uint id = Convert.ToUInt32(temp[0]["id"]);
            GPCMServer.DB.Execute("UPDATE namespace SET guid = @P0 WHERE id = @P1 ", guid.ToString(), id);
            GPCMServer.DB.Execute("UPDATE namespace SET sesskey = @P0 WHERE id = @P1 ", sesskey, id);
            return true;
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
            GPCMServer.DB.Execute("UPDATE profiles SET status=@P0,statstring =@P1,location = @P2",GPStatus.Offline,"","");
            GPCMServer.DB.Execute("UPDATE namespace SET guid = NULL");
        }
    }
}
