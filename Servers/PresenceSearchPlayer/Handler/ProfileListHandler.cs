using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    public class ProfileListHandler
    {
        public static void OnProfileList(GPSPClient client, Dictionary<string, string> dict)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger("profilelist", dict);
            GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
