using System;
using UniSpyLib.Abstraction.BaseClass.Redis;

namespace QueryReport.Entity.Structure.Misc
{
    public class PeerGroupInfoRedisKey : UniSpyRedisKeyBase
    {
        public string GameName { get; set; }
        public PeerGroupInfoRedisKey()
        {
        }
    }
}
