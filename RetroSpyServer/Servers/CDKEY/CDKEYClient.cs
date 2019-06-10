using System;
using System.Collections.Generic;
using GameSpyLib.Database;
using RetroSpyServer.DBQueries;
using GameSpyLib.Logging;
namespace RetroSpyServer.Servers.CDKEY
{
    public class CDKEYClient
    {
        private CDKEYDBQuery DBQuery;
        
        public CDKEYClient(DatabaseDriver dbdriver)
        {
            DBQuery = new CDKEYDBQuery(dbdriver);
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
