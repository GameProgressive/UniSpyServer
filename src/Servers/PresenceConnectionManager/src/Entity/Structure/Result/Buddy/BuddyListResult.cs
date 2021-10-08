using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal sealed class BuddyListResult : ResultBase
    {
        public List<uint> ProfileIDList;

        public BuddyListResult()
        {
            ProfileIDList = new List<uint>();
        }
    }
}
