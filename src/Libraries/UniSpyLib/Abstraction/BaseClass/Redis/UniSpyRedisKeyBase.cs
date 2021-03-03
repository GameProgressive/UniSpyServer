﻿using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UniSpyLib.Extensions;

namespace UniSpyLib.Abstraction.BaseClass.Redis
{
    public class UniSpyRedisKeyBase
    {
        [JsonIgnore]
        public string RedisSearchKey => BuildSearchKey();
        [JsonIgnore]
        public string RedisFullKey => BuildFullKey();
        [JsonIgnore]
        public RedisDataBaseNumber? DatabaseNumber { get; protected set; }
        [JsonIgnore]
        public TimeSpan? ExpireTime { get; protected set; }
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
            var pattern1 = new Regex("[ ',' , '{' , '}' ]");
            var pattern2 = new Regex("['\n',' ']");
            redisKey = pattern1.Replace(redisKey, "*");
            redisKey = pattern2.Replace(redisKey, "");
            return redisKey;
        }
    }
}