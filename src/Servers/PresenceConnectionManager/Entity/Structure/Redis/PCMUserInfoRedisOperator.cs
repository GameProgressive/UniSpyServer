using PresenceConnectionManager.Structure.Data;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace PresenceConnectionManager.Handler.SystemHandler.Redis
{
    internal sealed class PCMUserInfoRedisOperator :
        UniSpyRedisOperatorBase<PCMUserInfoRedisKey, PCMUserInfo>
    {
    }
}
