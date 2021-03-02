using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace QueryReport.Entity.Structure.Redis
{
    public class GameServerInfoRedisOperator :
        UniSpyRedisOperatorBase<GameServerInfoRedisKey,GameServerInfo>
    {
        static GameServerInfoRedisOperator() 
        {
            _dbNumber = RedisDataBaseNumber.GameServer;
        }
    }
}
