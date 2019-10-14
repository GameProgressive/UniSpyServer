using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.DelBuddy
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>
   public class DelBuddyHandler
    {
        GPCMSession _session;
        Dictionary<string, string> _recv;
        public static void Handle(GPCMSession session, Dictionary<string,string> recv)
        {

        }
    }
}
