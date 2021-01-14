using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal class BuddyListResult : PCMResultBase
    {
        public List<uint> ProfileIdList;

        public BuddyListResult()
        {
            ProfileIdList = new List<uint>();
        }
    }
}
