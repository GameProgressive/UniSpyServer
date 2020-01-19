using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Profile.RegisterNick;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.RegisterNick
{
    public class RegisterNickHandler
    {
        /// <summary>
        /// update the uniquenick
        /// </summary>
        /// <param name="session"></param>
        /// <param name="dict"></param>
        public static void RegisterNick(GPCMSession session, Dictionary<string, string> dict)
        {
            GPErrorCode error = IsContainAllKeys(dict);
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, error, "Parsing error");
                return;
            }
            string sendingBuffer;

            try
            {
                RegisterNickQuery.UpdateUniquenick(dict["uniquenick"],session.PlayerInfo.SessionKey,Convert.ToUInt16(dict["patnerid"]));
                sendingBuffer = @"\rn\final\";
                session.Send(sendingBuffer);
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
