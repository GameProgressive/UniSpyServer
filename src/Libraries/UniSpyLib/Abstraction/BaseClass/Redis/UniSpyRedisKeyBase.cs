using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace UniSpyLib.Abstraction.BaseClass.Redis
{
    public class UniSpyRedisKeyBase
    {
        [JsonIgnore]
        public string RedisSearchKey => BuildSearchKey();
        [JsonIgnore]
        public string RedisFullKey => BuildFullKey();
        public UniSpyRedisKeyBase()
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
            var pattern = new Regex("[ ',' , '{' , '}' ]");
            var searchKey = pattern.Replace(redisKey, "*");
            return searchKey;
        }
    }
}
