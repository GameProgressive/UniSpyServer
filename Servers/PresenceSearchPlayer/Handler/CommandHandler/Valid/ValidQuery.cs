using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler.Valid
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
            var result = GPSPServer.DB.Query("SELECT userid FROM users WHERE `email`=@P0", email);
            return (result.Count == 0) ? false : true;
        }
    }
}
