using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace UniSpyServer.LinqToRedis
{
    public abstract record RedisKeyValueObject : IRedisKey
    {

        // [JsonIgnore]
        // protected List<Type> _supportedTypes = new List<Type>
        // {
        //     typeof(string),
        //     typeof(int),
        //     typeof(long),
        //     typeof(double),
        //     typeof(bool),
        //     typeof(int?),
        //     typeof(int),
        //     typeof(int?),
        //     typeof(long?),
        //     typeof(double?),
        //     typeof(bool?),
        //     typeof(DateTime),
        //     typeof(byte[]),
        //     typeof(byte?[]),
        //     typeof(Guid?),
        //     typeof(Guid),
        //     typeof(TimeSpan),
        //     typeof(IPEndPoint),
        //     typeof(Nullable),
        //     typeof(Enum)
        // };
        [JsonIgnore]
        public TimeSpan? ExpireTime { get; private set; }
        [JsonIgnore]
        public string FullKey => BuildFullKey();
        [JsonIgnore]
        public string SearchKey => BuildSearchKey();
        public RedisKeyValueObject(TimeSpan? expireTime)
        {
            ExpireTime = expireTime;
        }
        private string BuildFullKey()
        {
            string fullKey = null;
            var properties = GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(RedisKeyAttribute), true).Select(a => a is RedisKeyAttribute).Any()).ToList();
            if (properties.Count() == 0)
            {
                throw new ArgumentNullException($"The RedisKeyValueObject:{this.GetType().Name} must have a key");
            }
            foreach (var property in properties)
            {
                // if (!_supportedTypes.Contains(property.PropertyType))
                // {
                //     throw new NotSupportedException($"The complex type:{property.PropertyType} is not supported");
                // }
                if (property.GetValue(this) == null)
                {
                    throw new ArgumentNullException($"{property.Name} is null when building full key.");
                }
                var keyValueStr = $"{property.Name}={property.GetValue(this)}";
                if (fullKey == null)
                {
                    fullKey = $"{keyValueStr}";
                }
                else
                {
                    fullKey = $"{fullKey}:{keyValueStr}";
                }
            }
            return fullKey;
        }
        private string BuildSearchKey()
        {
            var builder = new StringBuilder();
            var properties = GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(RedisKeyAttribute), false).Where(a => a is RedisKeyAttribute).Any());
            if (properties.Count() == 0)
            {
                throw new ArgumentNullException($"The RedisKeyValueObject:{this.GetType().Name} must have a key");
            }
            builder.Append("*");

            foreach (var property in properties)
            {
                if (property.GetValue(this) == null)
                {
                    continue;
                }
                else
                {
                    builder.Append($"{property.Name}={property.GetValue(this)}");
                }
                builder.Append("*");
            }
            return builder.ToString();
        }
    }
}