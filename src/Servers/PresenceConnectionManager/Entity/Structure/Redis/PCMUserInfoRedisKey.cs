using Newtonsoft.Json;
using System;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace PresenceConnectionManager.Handler.SystemHandler.Redis
{
    internal sealed class PCMUserInfoRedisKey : UniSpyRedisKey
    {
        [JsonProperty(Order = -2)]
        public Guid ServerID { get; set; }
        public string SessionHashValue { get; set; }
        public PCMUserInfoRedisKey()
        {
            DatabaseNumber = RedisDataBaseNumber.GamePresence;
        }
    }
}