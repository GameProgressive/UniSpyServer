using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    class ValidHandler
    {
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
                    if (GPSPHandler.DBQuery.IsEmailValid(dict["email"]))
                        client.Stream.SendAsync(@"\vr\1\final\");
                    else
                        client.Stream.SendAsync(@"\vr\0\final\");

                    client.Stream.Close();
                }
                else
                {
                    client.Stream.SendAsync(@"\vr\0\final\");
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
