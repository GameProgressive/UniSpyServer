using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler
{
    public class AddBuddyHandler
    {
        public static void Addfriends(GPCMClient client,Dictionary<string,string> dict)
        {
            GamespyUtils.PrintReceivedGPDictToLogger("profilelist", dict);
            GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }
    }
}
