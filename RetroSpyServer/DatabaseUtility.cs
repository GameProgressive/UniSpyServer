using System.Data.Common;
using System.Collections.Generic;
using GameSpyLib.Database;

namespace RetroSpyServer
{
    public class DatabaseUtility
    {
        public static Dictionary<string, object> GetUser(DatabaseDriver databaseDriver, string Unick)
        {
            var Rows = databaseDriver.Query("SELECT profiles.profileid, users.password, profiles.countrycode, profiles.status FROM player INNER JOIN users ON profiles.userid = users.userid WHERE profiles.uniquenick=@P0", Unick);
            return (Rows.Count == 0) ? null : Rows[0];
        }

        public static bool UserExists(DatabaseDriver databaseDriver, string Nick)
        {
            return (databaseDriver.Query("SELECT profileid FROM profiles WHERE `nickname`=@P0", Nick).Count != 0);
        }

        /// <summary>
        /// Creates a new Gamespy Account
        /// </summary>
        /// <remarks>Used by the login server when a create account request is made</remarks>
        /// <param name="databaseDriver">The database connection to use</param>
        /// <param name="Nick">The Account Name</param>
        /// <param name="Pass">The UN-HASHED Account Password</param>
        /// <param name="Email">The Account Email Address</param>
        /// <param name="Country">The Country Code for this Account</param>
        /// <param name="UniqueNick">The unique nickname for this Account</param>
        /// <returns>Returns the Player ID if sucessful, 0 otherwise</returns>
        public static uint CreateUser(DatabaseDriver databaseDriver, string Nick, string Pass, string Email, string Country, string UniqueNick)
        {
            databaseDriver.Execute("INSERT INTO users(email, password) VALUES(@P0, @P1)", Email, Pass);
            var Rows = databaseDriver.Query("SELECT userid FROM users WHERE email=@P0 and password=@P1", Email, Pass);
            if (Rows.Count < 1)
                return 0;

            databaseDriver.Execute("INSERT INTO profiles(userid, nick, uniquenick, countrycode) VALUES(@P0, @P1, @P2, @P3)", Rows[0]["userid"], Nick, UniqueNick, Country);
            Rows = databaseDriver.Query("SELECT profileid FROM profiles WHERE uniquenick=@P0", UniqueNick);
            if (Rows.Count < 1)
                return 0;

            return uint.Parse(Rows[0]["profileid"].ToString());
        }

        public static void UpdateUser(DatabaseDriver databaseDriver, uint playerId, string Country)
        {
            databaseDriver.Execute("UPDATE profiles SET countrycode=@P0 WHERE `profileid`=@P1", Country, playerId);
        }
    }
}
