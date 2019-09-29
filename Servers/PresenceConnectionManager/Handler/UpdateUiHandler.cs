using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler
{
    public class UpdateUiHandler
    {
        public static void UpdateUi(GPCMClient client, Dictionary<string, string> recv)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger("profilelist", recv);
            GameSpyUtils.SendGPError(client, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
