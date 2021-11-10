using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis
{
    public class GameServerInfoRedisOperator : UniSpyRedisOperator<GameServerInfoRedisKey, GameServerInfo>
    {
        static GameServerInfoRedisOperator()
        {
        }
    }
}
