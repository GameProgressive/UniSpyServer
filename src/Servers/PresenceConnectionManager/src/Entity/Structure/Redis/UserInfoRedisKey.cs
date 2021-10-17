using Newtonsoft.Json;
using System;
using UniSpyLib.Abstraction.BaseClass.Redis;

namespace PresenceConnectionManager.Handler.SystemHandler.Redis
{
    internal sealed class UserInfoRedisKey : UniSpyRedisKey
    {
        [JsonProperty(Order = -2)]
        public Guid ServerID { get; set; }
        public string SessionHashValue { get; set; }
        public UserInfoRedisKey()
        {
            Db = UniSpyLib.Extensions.DbNumber.GamePresence;
        }
    }
}