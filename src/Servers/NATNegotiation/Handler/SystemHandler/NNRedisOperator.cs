using System;
using System.Collections.Generic;
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace NATNegotiation.Handler.SystemHandler
{
    internal sealed class NNRedisOperator : UniSpyRedisOperatorBase<NatUserInfo>
    {
        public NNRedisOperator() : base(RedisDBNumber.NatNeg)
        {
        }

        public string BuildFullKey(IPEndPoint iPEndPoint, NatPortType natPortType, uint cookie)
        {
            return base.BuildFullKey(iPEndPoint, natPortType, cookie);
        }
        public string BuildSearchKey(NatPortType portType, uint cookie)
        {
            return base.BuildSearchKey(portType, cookie);
        }
        public string BuildSearchKey(IPEndPoint iPEndPoint, uint cookie)
        {
            return base.BuildSearchKey(iPEndPoint, cookie);
        }
        public string BuildSearchKey(IPEndPoint iPEndpoint, NatPortType natPortType)
        {
            return base.BuildSearchKey(iPEndpoint, natPortType);
        }
        public string BuildSearchKey(IPEndPoint iPEndPoint)
        {
            return base.BuildSearchKey(iPEndPoint);
        }
        public string BuildSearchKey(uint cookie)
        {
            return base.BuildSearchKey(cookie);
        }

        public override Dictionary<string, NatUserInfo> GetMatchedKeyValues(string searchKey)
        {
            return base.GetMatchedKeyValues(searchKey);
        }

        public override bool SetKeyValue(string key, NatUserInfo value)
        {
            return base.SetKeyValue(key, value);
        }

        public override NatUserInfo GetSpecificValue(string fullKey)
        {
            return base.GetSpecificValue(fullKey);
        }

        public override Dictionary<string, NatUserInfo> GetAllKeyValues()
        {
            return base.GetAllKeyValues();
        }
    }
}
