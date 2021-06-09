using UniSpyLib.Abstraction.BaseClass.Redis;

namespace QueryReport.Entity.Structure.Redis
{
    public class GameServerInfoRedisOperator :
        UniSpyRedisOperatorBase<GameServerInfoRedisKey, GameServerInfo>
    {
        static GameServerInfoRedisOperator()
        {
        }
    }
}
