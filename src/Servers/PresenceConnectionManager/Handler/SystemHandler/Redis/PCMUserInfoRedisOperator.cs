using System;
using System.Net;
using PresenceConnectionManager.Structure.Data;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace PresenceConnectionManager.Handler.SystemHandler.Redis
{
    internal class PCMUserInfoRedisOperator : UniSpyRedisOperatorBase<PCMUserInfo>
    {
        static PCMUserInfoRedisOperator()
        {
            _dbNumber = RedisDataBaseNumber.GamePresence;
        }
    }        
}
