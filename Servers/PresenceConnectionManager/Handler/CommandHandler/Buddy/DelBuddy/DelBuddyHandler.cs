using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.DelBuddy
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>
    public class DelBuddyHandler : CommandHandlerBase
    {
        //GPCMSession _session;
        //Dictionary<string, string> _recv;
        public DelBuddyHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }
        //delete friend in database then send bm_revoke message to friend
    }
}
