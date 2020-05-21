using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.DelBuddy
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>
    public class DelBuddyHandler : PCMCommandHandlerBase
    {
        //PCMSession _session;
        //Dictionary<string, string> _recv;

        //delete friend in database then send bm_revoke message to friend
        public DelBuddyHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }
    }
}
