using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.DatabaseQuery
{
    public class ValidQuery
    {
        /// <summary>
        /// If an email is existed in database, this will return TRUE
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmailValid(string email)
        {
            return GPSPServer.DB.Query("SELECT userid FROM users WHERE `email`=@P0", email).Count > 0;
        }
    }
}
