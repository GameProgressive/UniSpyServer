using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    class SearchHandler
    {
        public static void SearchProfile(GPSPClient client, Dictionary<string, string> dict)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger("search", dict);
            GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }
    }
}
