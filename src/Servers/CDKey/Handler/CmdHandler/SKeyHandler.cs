using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;

namespace CDKey.Handler.CmdHandler
{
    /// <summary>
    /// This class contians gamespy cdkey check functions  which help cdkeyserver to finish the cdkey check. 
    /// This class is used to simplify the functions in server class, separate the other utility function making  the main server logic clearer
    /// </summary>
    internal sealed class SKeyHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">MD5cdkey string</param>
        /// <returns></returns>
        public static void IsCDKeyValid(IUniSpySession session, Dictionary<string, string> recv)
        {
            //if (DBQuery.IsCDKeyValidate(recv["skey"]))
            //{
            //    string reply = string.Format(@"\uok\\cd\{0}\skey\{1}", recv["resp"].Substring(0, 32), recv["skey"]);

            //    server.SendAsync(endPoint, Enctypex.XorEncoding(reply,0));
            //}
            //else
            //{
            //    LogWriter.Log.Write( LogLevel.Debug, "Incomplete or Invalid CDKey Packet Received: {0}", recv);
            //    //TODO cdkey invalid response
            //}                
        }
    }
}
