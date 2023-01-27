using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class BuddyListResult : ResultBase
    {
        public List<int> ProfileIDList { get; set; }

        public BuddyListResult()
        {
        }
    }
}
