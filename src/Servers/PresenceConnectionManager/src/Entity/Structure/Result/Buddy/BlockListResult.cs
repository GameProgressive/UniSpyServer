using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class BlockListResult : ResultBase
    {
        public List<uint> ProfileIdList;

        public BlockListResult()
        {
        }
    }
}
