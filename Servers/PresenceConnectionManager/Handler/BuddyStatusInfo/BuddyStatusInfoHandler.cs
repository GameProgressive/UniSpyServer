using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.BuddyStatusInfo
{
    public class BuddyStatusInfoHandler
    {
        private static Dictionary<string, string> _recv;
        public static void SendBuddyStatusInfo(GPCMSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
        }
    }
}
