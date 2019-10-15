using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.SearchUnique
{
    public class SearchUniqueHandler
    {
        public static void SearchProfileWithUniquenick(GPSPSession session, Dictionary<string, string> dict)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger(dict);
            GameSpyUtils.SendGPError(session, GPErrorCode.General, "This request is not supported yet.");
        }
    }
}
