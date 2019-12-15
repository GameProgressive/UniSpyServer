using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.DelBuddy
{
    /// <summary>
    /// handles dell buddy request,remove friends from friends list
    /// </summary>
    public class DelBuddyHandler : GPCMHandlerBase
    {
        //GPCMSession _session;
        //Dictionary<string, string> _recv;
        public DelBuddyHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override void Handle(GPCMSession session)
        {
            base.Handle(session);
        }
    }
}
