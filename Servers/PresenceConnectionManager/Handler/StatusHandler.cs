using PresenceConnectionManager.Application;
using PresenceConnectionManager.DatabaseQuery;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler
{
    public class StatusHandler
    {

        public static void UpdateStatus(GPCMSession session,Dictionary<string, string> recv)
        {
            //TODO
            ushort testSK;

            if (!recv.ContainsKey("statstring") || !recv.ContainsKey("locstring") || !recv.ContainsKey("sesskey"))
                return;

            if (!ushort.TryParse(recv["sesskey"], out testSK))
                return; // Invalid session key

            if (testSK != session.PlayerInfo.SessionKey)
                return; // Are you trying to update another user?
            StatusQuery.UpdateStatus(recv,session.Id);
        }
    }
}
