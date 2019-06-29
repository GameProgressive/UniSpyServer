using System;
using System.Collections.Generic;
using GameSpyLib.Database;
using RetroSpyServer.DBQueries;
using GameSpyLib.Logging;
namespace RetroSpyServer.Servers.CDKEY
{
    /// <summary>
    /// This class contians gamespy cdkey check functions  which help cdkeyserver to finish the cdkey check. 
    /// This class is used to simplify the functions in server class, separate the other utility function making  the main server logic clearer
    /// </summary>
    public class CDKeyHelper
    {
        private CDKeyDBQuery DBQuery;
        
        public CDKeyHelper(DatabaseDriver dbdriver)
        {
            DBQuery = new CDKeyDBQuery(dbdriver);
        }

        /// <summary>
        /// Converts a received parameter array from the client string to a keyValue pair dictionary
        /// </summary>
        /// <param name="parts">The array of data from the client</param>
        /// <returns></returns>
        public Dictionary<string, string> ConvertToKeyValue(string[] parts)
        {
            Dictionary<string, string> dict = new NiceDictionary<string, string>();

            try
            {
                for (int i = 0; i < parts.Length; i += 2)
                {
                    if (!dict.ContainsKey(parts[i]))
                        dict.Add(parts[i], parts[i + 1]);
                }
            }
            catch (IndexOutOfRangeException) { }

            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">MD5cdkey string</param>
        /// <returns></returns>
        public bool IsCDKeyValid(string str)
        {
            if (DBQuery.IsCDKeyValidate(str))
                return true;
            else
                return false;
        }
    }
}
