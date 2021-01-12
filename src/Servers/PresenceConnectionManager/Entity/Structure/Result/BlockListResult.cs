using System;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal class BlockListResult : PCMResultBase
    {
        public List<uint> ProfileIdList;

        public BlockListResult()
        {
        }
    }
}
