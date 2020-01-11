using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.UpdateUI
{
    /// <summary>
    /// Update user information (email)
    /// </summary>
    public class UpdateUIHandler
    {
        public static void UpdateUI(GPCMSession client, Dictionary<string, string> recv)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger(recv);
            GameSpyUtils.SendGPError(client, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
