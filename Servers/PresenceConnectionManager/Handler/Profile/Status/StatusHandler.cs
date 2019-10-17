using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Net;

namespace PresenceConnectionManager.Handler.Status
{
    public class StatusHandler
    {
        static Dictionary<string, string> _recv;
        public static void UpdateStatus(GPCMSession session, Dictionary<string, string> recv)
        {
            //TODO
            _recv = recv;
            if (!IsContainAllKey())
            { return; }

            ushort sessionkey;
            if (!ushort.TryParse(recv["sesskey"], out sessionkey))
                return; // Invalid session key
            // Are you trying to update another user?
            //you can only update your status.?
            if (sessionkey != session.PlayerInfo.SessionKey)
                return;
            StatusQuery.UpdateStatus(session.PlayerInfo,Convert.ToUInt32(recv["status"]),recv["statstring"],recv["locstring"]);
        }

        private static bool IsContainAllKey()
        {
            if (!_recv.ContainsKey("statstring") || !_recv.ContainsKey("locstring") || !_recv.ContainsKey("sesskey") || !_recv.ContainsKey("status"))
                return false;
            else
                return true;
        }
    }
}
