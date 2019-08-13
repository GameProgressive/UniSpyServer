using GameSpyLib.Common;
using RetroSpyServer.Servers.GPSP.Enumerators;
using System.Collections.Generic;

namespace RetroSpyServer.Servers.GPCM.Handler
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
