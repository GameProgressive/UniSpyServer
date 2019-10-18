using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Profile.RegisterNick.Query;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.RegisterNick
{
    public class RegisterNickHandler
    {
        /// <summary>
        /// update the uniquenick
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void RegisterNick(GPCMSession client, Dictionary<string, string> dict)
        {
            GPErrorCode error = IsContainAllKeys(dict);
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(client, error, "Parsing error");
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
        public static GPErrorCode IsContainAllKeys(Dictionary<string, string> dict)
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
