using RetroSpyServer.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.GPCM.Handler
{
   public class StatusHandler
    {

        public static void UpdateStatus(GPCMClient client,Dictionary<string, string> dictionary, GPCMStatusChanged OnStatusChanged)
        {
            ushort testSK;

            if (!dictionary.ContainsKey("statstring") || !dictionary.ContainsKey("locstring") || !dictionary.ContainsKey("sesskey"))
                return;

            if (!ushort.TryParse(dictionary["sesskey"], out testSK))
                return; // Invalid session key

            if (testSK != client.SessionKey)
                return; // Are you trying to update another user?

            client.PlayerInfo.PlayerStatusString = dictionary["statstring"];
            client.PlayerInfo.PlayerLocation = dictionary["locstring"];

            OnStatusChanged?.Invoke(client);
        }
    }
}
