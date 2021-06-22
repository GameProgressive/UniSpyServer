using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal sealed class BlockListResult : PCMResultBase
    {
        public List<uint> ProfileIdList;

        public BlockListResult()
        {
        }
    }
}
