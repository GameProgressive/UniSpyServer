using System;
using Newtonsoft.Json;
using UniSpyLib.Abstraction.BaseClass.Redis;

namespace PresenceConnectionManager.Handler.SystemHandler.Redis
{
    internal sealed class PCMUserInfoRedisKey : UniSpyRedisKeyBase
    {
        [JsonProperty(Order = -2)]
        public Guid ServerID { get; set; }
        public PCMUserInfoRedisKey()
        {
        }
    }
}