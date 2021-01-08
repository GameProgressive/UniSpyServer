using System;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    public class BlockListResult : PCMResultBase
    {
        public List<uint> ProfileIdList;

        public BlockListResult()
        {
        }

        public BlockListResult(UniSpyRequestBase request) : base(request)
        {
        }
    }
}
