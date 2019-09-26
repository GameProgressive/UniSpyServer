using PresenceConnectionManager;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler
{
    public class InviteToHandler
    {
        //public static GPCMDBQuery DBQuery = null;
        public static void AddFriends(GPCMClient client, Dictionary<string, string> recv)
        {
           GPErrorCode error =  IsContainAllKeys(recv);
            if (error != GPErrorCode.NoError)
            {
                GameSpyLib.Common.GameSpyUtils.SendGPError(client.Stream, error, "Parsing error in request");
            }


        }
        public static GPErrorCode IsContainAllKeys(Dictionary<string,string> recv)
        {
            if (!recv.ContainsKey("products") || !recv.ContainsKey("sesskey"))
                return GPErrorCode.Parse;

            if (!recv.ContainsKey("sesskey"))
                return GPErrorCode.Parse ;

            return GPErrorCode.NoError;
           
        }
    }
}
