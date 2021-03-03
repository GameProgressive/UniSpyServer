using System;
using Newtonsoft.Json;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace PresenceConnectionManager.Handler.SystemHandler.Redis
{
    internal sealed class PCMUserInfoRedisKey : UniSpyRedisKeyBase
    {
        [JsonProperty(Order = -2)]
        public Guid ServerID { get; set; }
        public PCMUserInfoRedisKey()
        {
            DatabaseNumber = RedisDataBaseNumber.GamePresence;
        }
    }
}