using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.SendBuddies.Query
{
    public class SendBuddiesQuery
    {
        /// <summary>
        /// Find a user's profile based on his profileid
        /// </summary>
        /// <param name="profileid"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetProfile(uint profileid, uint namespaceid)
        {
            //TODO
            var result = GPCMServer.DB.Query(
                @"SELECT 
                profiles.nick,
                namespace.uniquenick,
                users.email,
                profiles.firstname,
                profiles.lastname,
                profiles.icquin, 
                profiles.homepage,
                profiles.zipcode,  
                profiles.countrycode,  
                profiles.longitude,
                profiles.latitude, 
                profiles.location,
                profiles.birthday, 
                profiles.birthmonth,
                profiles.birthyear, 
                profiles.sex, 
                profiles.publicmask, 
                profiles.aim,
                profiles.picture, 
                profiles.occupationid, 
                profiles.industryid,
                profiles.incomeid,
                profiles.marriedid,
                profiles.childcount, 
                profiles.interests1,
                profiles.ownership1,
                profiles.connectiontype 
                FROM profiles 
                LEFT JOIN namespace ON namespace.profileid = profiles.profileid
                LEFT JOIN users ON users.userid = profiles.userid
                WHERE profiles.profileid=@P0 and namespace.namespaceid=@P1", profileid, namespaceid);
            return (result.Count == 0) ? null : result[0];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recv"></param>
        /// <returns></returns>
        public static Dictionary<string, object> SearchBuddiesId(uint profileid, uint namespaceid)
        {
            var result = GPCMServer.DB.Query("SELECT targetid FROM friends WHERE profileid = @P0 AND namespaceid = @P1", profileid, namespaceid);
            return (result.Count == 0) ? null : result[0];
        }


        public static Dictionary<string, object> GetStatusInfo(uint profileid, uint namespaceid)
        {
            var result = GPCMServer.DB.Query(
                 @"SELECT 
                profiles.status,
                profiles.statstring,
                profiles.location,
                users.lastip,
                namespace.port,
                profiles.quietflags
                FROM profiles 
                LEFT JOIN users on users.userid=profiles.profileid 
                LEFT JOIN namespace on namespace.profileid = profiles.profileid 
                WHERE namespace.profileid=@P0 AND namespace.namespaceid=@P1",
                 profileid, namespaceid
                 );
            return (result.Count == 0) ? null : result[0];
        }
    }
}
