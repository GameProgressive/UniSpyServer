using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure;
using System.Collections.Generic;
using System.Net;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace NATNegotiation.Handler.SystemHandler
{
    internal sealed class NatUserInfoRedisOperator : UniSpyRedisOperatorBase<NatUserInfo>
    {
        static NatUserInfoRedisOperator()
        {
            _dbNumber = RedisDBNumber.NatNeg;
        }

        public static string BuildFullKey(IPEndPoint iPEndPoint, NatPortType natPortType, uint cookie)
        {
            return UniSpyRedisOperatorBase<NatUserInfo>.BuildFullKey(iPEndPoint, natPortType, cookie);
        }
        public static string BuildSearchKey(NatPortType portType, uint cookie)
        {
            return UniSpyRedisOperatorBase<NatUserInfo>.BuildSearchKey(portType, cookie);
        }
        public static string BuildSearchKey(IPEndPoint iPEndPoint, uint cookie)
        {
            return UniSpyRedisOperatorBase<NatUserInfo>.BuildSearchKey(iPEndPoint, cookie);
        }
        public static string BuildSearchKey(IPEndPoint iPEndpoint, NatPortType natPortType)
        {
            return UniSpyRedisOperatorBase<NatUserInfo>.BuildSearchKey(iPEndpoint, natPortType);
        }
        public static string BuildSearchKey(IPEndPoint iPEndPoint)
        {
            return UniSpyRedisOperatorBase<NatUserInfo>.BuildSearchKey(iPEndPoint);
        }
        public static string BuildSearchKey(uint cookie)
        {
            return UniSpyRedisOperatorBase<NatUserInfo>.BuildSearchKey(cookie);
        }
    }
}
