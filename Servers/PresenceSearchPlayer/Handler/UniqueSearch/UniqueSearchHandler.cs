using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.UniqueSearch
{
    class UniqueSearchHandler
    {
        /// <summary>
        /// we just simply check the existance of the unique nickname in the database and suggest some numbered postfix nickname
        /// </summary>
        /// <param name="session"></param>
        /// <param name="dict"></param>
        public static void SuggestUniqueNickname(GPSPSession session, Dictionary<string, string> dict)
        {
            //The multiple nick suggest correct response is like 
            //@"\us\<number of suggested nick>\nick\<nick1>\nick\<nick2>\usdone\final\";
            string sendingBuffer;          
            GPErrorCode error = IsContainAllKeys(dict);
            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            if (UniqueSearchQuery.IsUniqueNickExist(dict))
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.General, "The Nick is existed, please choose another name");
            }
            else
            {
                sendingBuffer = @"\us\1\nick\" + dict["preferrednick"] + @"\usdone\final\";
                session.Send(sendingBuffer);
            }
        }

        public static GPErrorCode IsContainAllKeys(Dictionary<string, string> dict)
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
