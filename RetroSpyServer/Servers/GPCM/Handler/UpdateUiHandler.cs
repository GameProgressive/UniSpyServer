using GameSpyLib.Common;
using RetroSpyServer.Servers.GPSP.Enumerators;
using System.Collections.Generic;

namespace RetroSpyServer.Servers.GPCM.Handler
{
    public class UpdateUiHandler
    {
        public static void UpdateUi(GPCMClient client, Dictionary<string, string> dict)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger("profilelist", dict);
            GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
