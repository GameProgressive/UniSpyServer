using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using GameSpyLib.Database;
using System;

namespace RetroSpyServer
{
    public class DatabaseUtility
    {
        public static Dictionary<string, object> GetUserFromUniqueNick(DatabaseDriver databaseDriver, string Unick)
        {
            var Rows = databaseDriver.Query("SELECT profiles.profileid, users.password, profiles.countrycode, profiles.status, users.email, profiles.nick, users.userstatus FROM profiles INNER JOIN users ON profiles.userid = users.userid WHERE profiles.uniquenick=@P0", Unick);
            return (Rows.Count == 0) ? null : Rows[0];
        }

        public static Dictionary<string, object> GetUserFromNickname(DatabaseDriver databaseDriver, string Email, string Nick)
        {
            var Rows = databaseDriver.Query("SELECT profiles.profileid, users.password, profiles.countrycode, profiles.status, profiles.uniquenick, users.userstatus FROM profiles INNER JOIN users ON profiles.userid = users.userid WHERE profiles.nick=@P0 AND users.email=@P1", Nick, Email);
            return (Rows.Count == 0) ? null : Rows[0];
        }

        public static bool UserExists(DatabaseDriver databaseDriver, string Nick)
        {
            return (databaseDriver.Query("SELECT profileid FROM profiles WHERE `nickname`=@P0", Nick).Count != 0);
        }

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
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
            MD5 md5Hash = MD5.Create();
            databaseDriver.Execute("INSERT INTO users(email, password) VALUES(@P0, @P1)", Email, GetMd5Hash(md5Hash, Pass));
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

        public static void LoginUser(DatabaseDriver databaseDriver, uint playerId, ushort sessionKey)
        {
            databaseDriver.Execute("UPDATE profiles SET status=1, sesskey=@P0 WHERE profileid=@P1", sessionKey, playerId);
        }

        public static void LogoutUser(DatabaseDriver databaseDriver, uint playerId)
        {
            databaseDriver.Execute("UPDATE profiles SET status=0, sesskey=NULL WHERE profileid=@P1", playerId);
        }
    }
}