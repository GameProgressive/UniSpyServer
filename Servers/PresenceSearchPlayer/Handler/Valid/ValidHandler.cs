using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Valid
{
    class ValidHandler
    {
        /// <summary>
        /// check if a email is exist in database
        /// </summary>
        /// <param name="session"></param>
        /// <param name="dict"></param>
        public static void IsEmailValid(GPSPSession session, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            try
            {
                if (GameSpyUtils.IsEmailFormatCorrect(dict["email"]))
                {
                    if (ValidQuery.IsEmailValid(dict))
                        session.Send(@"\vr\1\final\");
                    else
                        session.Send(@"\vr\0\final\");

                    //client.Stream.Dispose();
                }
                else
                {
                    session.Send(@"\vr\0\final\");
                    //client.Stream.Dispose();
                }

            }
            catch (Exception ex)
            {
                LogWriter.Log.WriteException(ex);
                GameSpyUtils.SendGPError(session, GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
            }
        }
    }
}
