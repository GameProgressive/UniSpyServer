using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.DatabaseQuery
{
    public class SendBuddiesQuery
    {
        public static Dictionary<string, object> GetProfile(uint profileid)
        {
            //TODO
            GameSpyLib.Logging.LogWriter.Log.Write("Not supported yet",GameSpyLib.Logging.LogLevel.Error);
            return null;
           

        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recv"></param>
        /// <returns></returns>
        public static int[] GetProfileidArray(Dictionary<string,string> recv)
        {
            //use namespaceid,productid,gamename to find friends pid
            throw new NotImplementedException();
        }
    }
}
