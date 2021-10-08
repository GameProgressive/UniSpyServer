using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace QueryReport.Entity.Structure.Redis
{
    public class PeerGroupInfoRedisKey : UniSpyRedisKey
    {
        public string GameName { get; set; }
        public PeerGroupInfoRedisKey()
        {
            Db = UniSpyLib.Extensions.DbNumber.PeerGroup;
        }
    }
}
