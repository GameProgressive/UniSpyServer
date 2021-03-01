using NATNegotiation.Entity.Structure;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace NATNegotiation.Handler.SystemHandler.Redis
{
    internal sealed class NatUserInfoRedisOperator : UniSpyRedisOperatorBase<NatUserInfo>
    {
        static NatUserInfoRedisOperator()
        {
            _dbNumber = RedisDataBaseNumber.NatNeg;
        }
    }
}
