using Newtonsoft.Json;
using System;
using UniSpyLib.Extensions;

namespace UniSpyLib.Abstraction.BaseClass.Redis
{
    public abstract class UniSpyRedisKey
    {
        [JsonIgnore]
        public string RedisSearchKey => BuildSearchKey();
        [JsonIgnore]
        public string RedisFullKey => BuildFullKey();
        [JsonIgnore]
        public RedisDataBaseNumber? DatabaseNumber { get; protected set; }
        [JsonIgnore]
        public TimeSpan? ExpireTime { get; protected set; }
        public UniSpyRedisKey()
        {
        }

        private string BuildFullKey()
        {
            foreach (var property in GetType().GetFields())
            {
                if (property.GetValue(this) == null)
                {
                    throw new ArgumentNullException();
                }
            }
            var redisKey = JsonConvert.SerializeObject(this);
            return redisKey;
        }

        private string BuildSearchKey()
        {
            var redisKey = JsonConvert.SerializeObject(this);
            redisKey = redisKey.Replace("{", "*").Replace("}", "*")
            .Replace(",", "*").Replace("\n", "");

            return redisKey;
        }
    }
}
