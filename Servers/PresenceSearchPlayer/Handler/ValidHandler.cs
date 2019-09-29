using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.DatabaseQuery;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    class ValidHandler
    {
        /// <summary>
        /// check if a email is exist in database
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void IsEmailValid(GPSPClient client, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            try
            {
                if (GameSpyUtils.IsEmailFormatCorrect(dict["email"]))
                {
                    if (ValidQuery.IsEmailValid(dict))
                        client.Send(@"\vr\1\final\");
                    else
                        client.Send(@"\vr\0\final\");

                    client.Stream.Close();
                }
                else
                {
                    client.Send(@"\vr\0\final\");
                    client.Stream.Close();
                }

            }
            catch (Exception ex)
            {
                LogWriter.Log.WriteException(ex);
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
            }
        }
    }
}
