using UniSpyLib.Abstraction.BaseClass.Redis;

namespace QueryReport.Entity.Structure.Redis
{
    public class GameServerInfoRedisOperator :
        UniSpyRedisOperator<GameServerInfoRedisKey, GameServerInfo>
    {
        static GameServerInfoRedisOperator()
        {
        }
    }
}
