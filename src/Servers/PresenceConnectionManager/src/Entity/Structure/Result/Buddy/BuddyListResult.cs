using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class BuddyListResult : ResultBase
    {
        public List<int> ProfileIDList;

        public BuddyListResult()
        {
            ProfileIDList = new List<int>();
        }
    }
}
