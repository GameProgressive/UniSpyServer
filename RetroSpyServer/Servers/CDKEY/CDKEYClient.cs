using System;
using System.Collections.Generic;
using GameSpyLib.Network;
namespace RetroSpyServer.Servers.CDKEY
{
    public class CDKEYClient
    {
        public GameSpyUDPHandler handler { get; protected set; }
        public CDKEYClient(string decryptedClientData)
        {
            ProcessDataReceived(decryptedClientData);
        }

        protected void ProcessDataReceived(string decryptedClientData)
        {            
            //implement checking sdk in the database
        }
        /// <summary>
        /// Converts a received parameter array from the client string to a keyValue pair dictionary
        /// </summary>
        /// <param name="parts">The array of data from the client</param>
        /// <returns></returns>
        public Dictionary<string, string> ConvertToKeyValue(string[] parts)
        {
            Dictionary<string, string> Dic = new NiceDictionary<string, string>();

            try
            {
                for (int i = 0; i < parts.Length; i += 2)
                {
                    if (!Dic.ContainsKey(parts[i]))
                        Dic.Add(parts[i], parts[i + 1]);
                }
            }
            catch (IndexOutOfRangeException) { }

            return Dic;
        }       
    }
}
