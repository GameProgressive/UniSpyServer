using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class BlockListResult : ResultBase
    {
        public List<int> ProfileIdList;

        public BlockListResult()
        {
        }
    }
}
