using System;
using System.Collections.Generic;
using GameSpyLib.Database;
using RetroSpyServer.DBQueries;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using GameSpyLib.Common;
using System.Text;
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
        public void IsCDKeyValid(CDKeyServer server,UdpPacket packet, string[] str)
        {
            Dictionary<string, string> recv = GamespyUtils.ConvertGPResponseToKeyValue(str);
            if (DBQuery.IsCDKeyValidate(recv["skey"]))
            {
                string reply = String.Format(@"\uok\\cd\{0}\skey\{1}", recv["resp"].Substring(0, 32), recv["skey"]);
                packet.SetBufferContents(Encoding.UTF8.GetBytes(Xor(reply)));
                server.ReplyAsync(packet);
            }
            else
            {
                //TODO cdkey invalid response
            }
                
        }

        /// <summary>
        /// Encrypts / Descrypts the CDKey Query String
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string Xor(string s)
        {
            const string gamespy = "gamespy";
            int length = s.Length;
            char[] data = s.ToCharArray();
            int index = 0;

            for (int i = 0; length > 0; length--)
            {
                if (i >= gamespy.Length)
                    i = 0;

                data[index++] ^= gamespy[i++];
            }

            return new String(data);
        }

    }
}
