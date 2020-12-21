using System;
using QueryReport.Entity.Structure.Group;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;

namespace QueryReport.Handler.SystemHandler.Redis
{
    public class PeerGroupInfoRedisOperator : UniSpyRedisOperatorBase<PeerGroupInfo>
    {
        public PeerGroupInfoRedisOperator() : base(RedisDBNumber.PeerGroup)
        {
        }

        public override PeerGroupInfo GetSpecificValue(string fullKey)
        {
            return base.GetSpecificValue(fullKey);
        }

        public override bool SetKeyValue(string key, PeerGroupInfo value)
        {
            return base.SetKeyValue(key, value);
        }

        public string BuildSearchKey(string gameName)
        {
            return base.BuildSearchKey(gameName);
        }
    }
}