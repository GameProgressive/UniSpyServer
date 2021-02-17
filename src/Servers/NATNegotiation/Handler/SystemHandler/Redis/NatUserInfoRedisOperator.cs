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
            return BuildFullKey(
                $"IPEndPoint:{iPEndPoint}",
                $"NatPortType:{natPortType}",
                $"Cookie:{cookie}");
        }
        public static string BuildSearchKey(NatPortType natPortType, uint cookie)
        {
            return BuildSearchKey(
                $"NatPortType:{natPortType}",
                $"Cookie:{cookie}");
        }
        public static string BuildSearchKey(IPEndPoint iPEndPoint, uint cookie)
        {
            return BuildSearchKey(
                $"IPEndPoint:{iPEndPoint}",
                $"Cookie:{cookie}");
        }
        public static string BuildSearchKey(IPEndPoint iPEndPoint, NatPortType natPortType)
        {
            return BuildSearchKey(
                $"IPEndPoint:{iPEndPoint}",
                $"NatPortType:{natPortType}");
        }
        public static string BuildSearchKey(IPEndPoint iPEndPoint)
        {
            return BuildSearchKey($"IPEndPoint:{iPEndPoint}");
        }
        public static string BuildSearchKey(uint cookie)
        {
            return BuildSearchKey($"Cookie:{cookie}");
        }
    }
}
