using GameSpyLib.Common;
using PresenceSearchPlayer.DatabaseQuery;
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
            GPErrorCode error = IsContainAllKeys(dict);
            if (error!=GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(client, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            if (UniqueSearchQuery.IsUniqueNickExist(dict))
            {
                GameSpyUtils.SendGPError(client, GPErrorCode.General, "The Nick is existed, please choose another name");
            }
            else
            {
                sendingBuffer = @"\us\1\nick\" + dict["preferrednick"] + @"\usdone\final\";
                client.Send(sendingBuffer);
            }
        }

        public static GPErrorCode IsContainAllKeys(Dictionary<string,string> dict)
        {
            if (!dict.ContainsKey("preferrednick"))
                return GPErrorCode.Parse;
            if (!dict.ContainsKey("namespaceid"))
            {
                dict.Add("namespaceid", "0");
            }
            return GPErrorCode.NoError;
        }
    }
}
