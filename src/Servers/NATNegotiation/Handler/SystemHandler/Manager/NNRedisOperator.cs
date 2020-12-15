using System.Collections.Generic;
using System.Net;
using NATNegotiation.Entity.Structure;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace NATNegotiation.Handler.SystemHandler.Manager
{
    public class NNRedisOperator : UniSpyRedisOperatorBase
    {
        public NNRedisOperator() : base(RedisDBNumber.NatNeg)
        {
        }

        public bool SetKeyValue(string key, NatUserInfo value)
        {
            return BasicSetKeyValue(key, value);
        }
        public NatUserInfo GetValue(string key)
        {
            return BasicGetValue<NatUserInfo>(key);
        }

        public Dictionary<string, NatUserInfo> GetAllKeyValues()
        {
            return GetAllKeyValues<NatUserInfo>();
        }

        public string GenerateKeyName(EndPoint endPoint, uint cookie)
        {
            return $"{(IPEndPoint)endPoint} {cookie}";
        }
    }
}
