using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler
{
    public class UpdateUiHandler
    {
        public static void UpdateUi(GPCMSession client, Dictionary<string, string> recv)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger( recv);
            GameSpyUtils.SendGPError(client, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
