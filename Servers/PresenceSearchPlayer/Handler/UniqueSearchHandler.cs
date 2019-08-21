using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    class UniqueSearchHandler
    {
        /// <summary>
        /// we just simply check the existance of the unique nickname in the database and suggest some numbered postfix nickname
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void SuggestUniqueNickname(GPSPClient client, Dictionary<string, string> dict)
        {
            //The multiple nick suggest correct response is like 
            //@"\us\<number of suggested nick>\nick\<nick1>\nick\<nick2>\usdone\final\";
            string sendingBuffer;
            if (!dict.ContainsKey("preferrednick"))
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            if (!GPSPHandler.DBQuery.IsUniqueNickExistForSuggest(dict))
            {
                sendingBuffer = @"\us\1\nick\" + dict["preferrednick"] + @"\usdone\final\";
                client.Stream.SendAsync(sendingBuffer);
            }
            else
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "The Nick is existed, please choose another name");
            }
        }


    }
}
