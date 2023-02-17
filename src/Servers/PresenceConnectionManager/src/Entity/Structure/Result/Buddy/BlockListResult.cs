using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class BlockListResult : ResultBase
    {
        public List<int> ProfileIdList;

        public BlockListResult()
        {
        }
    }
}
