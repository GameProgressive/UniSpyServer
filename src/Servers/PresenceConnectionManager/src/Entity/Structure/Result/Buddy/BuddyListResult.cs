using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class BuddyListResult : ResultBase
    {
        public List<uint> ProfileIDList;

        public BuddyListResult()
        {
            ProfileIDList = new List<uint>();
        }
    }
}
