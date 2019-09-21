using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.DatabaseQuery;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

/////////////////////////Finished/////////////////////////////////
namespace PresenceSearchPlayer.Handler
{

    /// <summary>
    /// Uses a nick to find how many uniquenick is in this nick
    /// </summary>
    public class NickHandler
    {

        /// <summary>
        /// Get nickname through email and password
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void SearchNicks(GPSPClient client, Dictionary<string, string> dict)
        {
            //Format the password for our database storage
            //if not recieved correct request we terminate
            GPErrorCode error = IsSearchNicksContianAllKeys(dict);
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(client.Stream, (int)error, "Error recieving SearchNicks request.");
                return;
            }

            List<Dictionary<string, object>> queryResult;

            try
            {
                //get nicknames from GPSPDBQuery class
                queryResult = NickQuery.RetriveNicknames(dict);
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Error);
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
                return;
            }

            if (queryResult.Count < 1)
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "No match found !");
                return;
            }

            string sendingBuffer;
            sendingBuffer = @"\nr\";
            foreach (Dictionary<string, object> row in queryResult)
            {
                sendingBuffer += @"\nick\";
                sendingBuffer += row["nick"];
                sendingBuffer += @"\uniquenick\";
                sendingBuffer += row["uniquenick"];

            }

            sendingBuffer += @"\ndone\final\";
            client.Stream.SendAsync(sendingBuffer);
        }

        public static GPErrorCode IsSearchNicksContianAllKeys(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {

                return GPErrorCode.Parse;
            }

            // First, we try to receive an encoded password
            if (!dict.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!dict.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue                   
                    return GPErrorCode.Parse;
                }
            }
            return GPErrorCode.NoError;
        }
    }
}
