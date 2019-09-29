using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceConnectionManager.DatabaseQuery;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler
{
    public class RegisterNickHandler
    {
        /// <summary>
        /// update the uniquenick
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void RegisterNick(GPCMClient client,Dictionary<string,string> dict)
        {
            GPErrorCode error = IsContainAllKeys(dict);
            if(error!=GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(client.Stream,error,"Parsing error");
                return;
            }
            string sendingBuffer;
            
            try
            {
                 RegisterNickQuery.UpdateUniquenick(dict);              
                    sendingBuffer = @"\rn\final\";
                    client.Send(sendingBuffer);              
            } 
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
           
          

        }
        public static GPErrorCode IsContainAllKeys(Dictionary<string,string> dict)
        {
            if (!dict.ContainsKey("sesskey"))
            { 
                return GPErrorCode.Parse;
            }
            if (!dict.ContainsKey("uniquenick"))
            {
                return GPErrorCode.Parse;
            }
            return GPErrorCode.NoError;
        }
    }
}
