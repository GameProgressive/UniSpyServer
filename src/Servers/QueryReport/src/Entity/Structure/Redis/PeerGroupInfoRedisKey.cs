using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis
{
    public class PeerGroupInfoRedisKey : UniSpyRedisKey
    {
        public string GameName { get; set; }
        public PeerGroupInfoRedisKey()
        {
            Db = UniSpyServer.UniSpyLib.Extensions.DbNumber.PeerGroup;
        }
    }
}
