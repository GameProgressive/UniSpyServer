using System;
using System.Collections.Generic;
using GameSpyLib.Network;
namespace RetroSpyServer.Servers.CDKEY
{
    public class CDKEYClient
    {
        private GameSpyUDPConnector connector;
        public GameSpyUDPHandler handler { get; protected set; }
        public CDKEYClient(string decryptedClientData)
        {
            ProcessDataReceived(decryptedClientData);
        }

        public CDKEYClient(GameSpyUDPConnector parent,GameSpyUDPHandler handler)
        {
            connector = parent;
            this.handler = handler;

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
    }
}
