using System;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

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
