using PresenceConnectionManager.Application;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler
{
    public class StatusHandler
    {

        public static void UpdateStatus(GPCMClient client,Dictionary<string, string> recv, GPCMStatusChanged OnStatusChanged)
        {
            //TODO
            ushort testSK;

            if (!recv.ContainsKey("statstring") || !recv.ContainsKey("locstring") || !recv.ContainsKey("sesskey"))
                return;

            if (!ushort.TryParse(recv["sesskey"], out testSK))
                return; // Invalid session key

            if (testSK != client.SessionKey)
                return; // Are you trying to update another user?

            client.PlayerInfo.PlayerStatusString = recv["statstring"];
            client.PlayerInfo.PlayerLocation = recv["locstring"];

            OnStatusChanged?.Invoke(client);
        }
    }
}
