using System;
using System.Collections.Generic;
using System.Net;
using QueryReport.Entity.Structure;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
namespace QueryReport.Handler.SystemHandler.Redis
{
    public class GameServerInfoRedisOperator : UniSpyRedisOperatorBase<GameServerInfo>
    {
        public GameServerInfoRedisOperator() : base(RedisDBNumber.GameServer)
        {
        }

        public string BuildFullKey(IPEndPoint iPEndPoint, string gameName)
        {
            return base.BuildFullKey(iPEndPoint, gameName);
        }
        public string BuildSearchKey(IPEndPoint iPEndPoint, string gameName)
        {
            return base.BuildSearchKey(iPEndPoint, gameName);
        }
        public string BuildSearchKey(IPEndPoint iPEndPoint)
        {
            return base.BuildSearchKey(iPEndPoint);
        }
        public string BuildSearchKey(string gameName)
        {
            return base.BuildSearchKey(gameName);
        }
        public override Dictionary<string, GameServerInfo> GetMatchedKeyValues(string searchKey)
        {
            return base.GetMatchedKeyValues(searchKey);
        }

        public override GameServerInfo GetSpecificValue(string fullKey)
        {
            return base.GetSpecificValue(fullKey);
        }

        public override bool SetKeyValue(string key, GameServerInfo value)
        {
            return base.SetKeyValue(key, value);
        }


    }
}
