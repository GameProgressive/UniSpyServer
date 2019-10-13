using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.DatabaseQuery
{
    public class SendBuddiesQuery
    {
        /// <summary>
        /// Find a user's profile based on his profileid
        /// </summary>
        /// <param name="profileid"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetProfile(uint profileid,uint namespaceid)
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
        public static int[] GetProfileidArray(Dictionary<string,string> recv)
        {
            //use namespaceid,productid,gamename to find friends pid
            throw new NotImplementedException();
        }
    }
}
