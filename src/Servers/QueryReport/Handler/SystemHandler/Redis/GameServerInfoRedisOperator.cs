using QueryReport.Entity.Structure;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace QueryReport.Handler.SystemHandler.Redis
{
    public sealed class GameServerInfoRedisOperator : UniSpyRedisOperatorBase<GameServerInfo>
    {
        static GameServerInfoRedisOperator() 
        {
            _dbNumber = RedisDBNumber.GameServer;
            _timeSpan = null;
        }

        public static string BuildFullKey(IPEndPoint iPEndPoint, string gameName)
        {
            return UniSpyRedisOperatorBase<GameServerInfo>.BuildFullKey(iPEndPoint, gameName);
        }
        public static string BuildSearchKey(IPEndPoint iPEndPoint, string gameName)
        {
            return UniSpyRedisOperatorBase<GameServerInfo>.BuildSearchKey(iPEndPoint, gameName);
        }
        public static string BuildSearchKey(IPEndPoint iPEndPoint)
        {
            return UniSpyRedisOperatorBase<GameServerInfo>.BuildSearchKey(iPEndPoint);
        }
        public static string BuildSearchKey(string gameName)
        {
            return UniSpyRedisOperatorBase<GameServerInfo>.BuildSearchKey(gameName);
        }
    }
}
