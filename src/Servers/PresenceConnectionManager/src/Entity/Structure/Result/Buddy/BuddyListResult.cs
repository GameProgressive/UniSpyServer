using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class BuddyListResult : ResultBase
    {
        public List<int> ProfileIDList { get; set; }

        public BuddyListResult()
        {
        }
    }
}
