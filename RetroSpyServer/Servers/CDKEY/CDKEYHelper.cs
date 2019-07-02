using System;
using System.Collections.Generic;
using GameSpyLib.Database;
using RetroSpyServer.DBQueries;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
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
                packet.SetBufferContents(Encoding.UTF8.GetBytes(Enctypex.XOR(reply)));
                server.ReplyAsync(packet);
            }
            else
            {
                //TODO cdkey invalid response
            }                
        }       

    }
}
