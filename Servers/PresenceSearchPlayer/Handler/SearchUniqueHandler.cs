using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler
{
    public class SearchUniqueHandler
    {
        public static void SearchProfileWithUniquenick(GPSPClient client, Dictionary<string, string> dict)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger("searchunique", dict);
            GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }
    }
}
