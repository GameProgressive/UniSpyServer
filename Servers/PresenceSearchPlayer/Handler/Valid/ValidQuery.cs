using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler.Valid
{
    public class ValidQuery
    {
        /// <summary>
        /// If an email is existed in database, this will return TRUE
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmailValid(Dictionary<string,string> dict)
        {
            var result =  GPSPServer.DB.Query("SELECT userid FROM users WHERE `email`=@P0", dict["email"]);
             return (result.Count == 0) ? false : true;
        }
    }
}
