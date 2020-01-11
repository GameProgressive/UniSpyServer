using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler.Check
{
    class CheckQuery
    {
        public static List<Dictionary<string,object>> GetProfileidFromNickEmailPassword(string email,string passenc,string nick)
        {           
            var result = GPSPServer.DB.Query("SELECT profileid FROM profiles " +
                " INNER JOIN users ON users.userid=profiles.userid " +
                " WHERE users.email=@P0 AND " +
                "users.password = @P1 AND " +
                "profiles.nick = @P2", email,passenc, nick);
            return (result.Count == 0) ? null : result;
        }

        public static bool FindEmail(string email)
        {
            var result = GPSPServer.DB.Query("SELECT userid FROM users WHERE users.email = @P0", email);
            return (result.Count == 0) ? false : true;
        }

        public static bool FindNick(string nick)
        {
            var result = GPSPServer.DB.Query("SELECT profileid FROM profiles WHERE profiles.nick = @P0", nick);
            return (result.Count == 0) ? false : true;
        }
        public static bool CheckPassword(string email,string password)
        {
            var result = GPSPServer.DB.Query("SELECT userid FROM users WHERE users.email = @P0 AND users.password = @P1", email,password);
            return (result.Count == 0) ? false : true;
        }
    
    }
}
