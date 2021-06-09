using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace QueryReport.Entity.Structure.Redis
{
    public class PeerGroupInfoRedisKey : UniSpyRedisKeyBase
    {
        public string GameName { get; set; }
        public PeerGroupInfoRedisKey()
        {
            DatabaseNumber = RedisDataBaseNumber.PeerGroup;
        }
    }
}
