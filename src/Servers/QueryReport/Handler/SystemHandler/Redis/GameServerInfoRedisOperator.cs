using QueryReport.Entity.Structure;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace QueryReport.Handler.SystemHandler.Redis
{
    public sealed class GameServerInfoRedisOperator : UniSpyRedisOperatorBase<GameServerInfo>
    {
        static GameServerInfoRedisOperator() 
        {
            _dbNumber = RedisDataBaseNumber.GameServer;
            _timeSpan = null;
        }
    }
}
