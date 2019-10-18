using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.AddBuddy
{
    public class AddBuddyHandler
    {
        public static void Addfriends(GPCMSession client, Dictionary<string, string> recv)
        {
            GameSpyUtils.PrintReceivedGPDictToLogger(recv);
            GameSpyUtils.SendGPError(client, GPErrorCode.General, "This request is not supported yet.");
        }
    }
}
