using System;
using System.Collections.Generic;
using GameSpyLib.Database;
using RetroSpyServer.DBQueries;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
using System.Text;
namespace RetroSpyServer.Servers.CDKey
{
    /// <summary>
    /// This class contians gamespy cdkey check functions  which help cdkeyserver to finish the cdkey check. 
    /// This class is used to simplify the functions in server class, separate the other utility function making  the main server logic clearer
    /// </summary>
    public class CDKeyHelper
    {

        public static CDKeyDBQuery DBQuery = null;
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">MD5cdkey string</param>
        /// <returns></returns>
        public static void IsCDKeyValid(CDKeyServer server,UdpPacket packet, Dictionary<string, string> recv)
        {
            if (DBQuery.IsCDKeyValidate(recv["skey"]))
            {
                string reply = string.Format(@"\uok\\cd\{0}\skey\{1}", recv["resp"].Substring(0, 32), recv["skey"]);
                packet.SetBufferContents(Encoding.UTF8.GetBytes(Enctypex.XOR(reply)));
                server.ReplyAsync(packet);
            }
            else
            {
                LogWriter.Log.Write("Incomplete or Invalid CDKey Packet Received: {0}", LogLevel.Debug, recv);
                //TODO cdkey invalid response
            }                
        }
        public static void DisconnectRequest(UdpPacket packet, Dictionary<string, string> recv)
        {
            // Handle, User disconnected from server
        }

        public static void InvalidCDKeyRequest(CDKeyServer server,UdpPacket packet, Dictionary<string, string> recv)
        {
            LogWriter.Log.Write("[CDKey] recieved Incomplete or Invalid  data : {0}", LogLevel.Debug, recv);
        }
    }
}
