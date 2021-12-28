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
        public RedisKeyValueObject(TimeSpan? expireTime)
        {
            ExpireTime = expireTime;
        }
        [JsonIgnore]
        private static List<Type> _supportedTypes = new List<Type>
        {
            typeof(string),
            typeof(int),
            typeof(long),
            typeof(double),
            typeof(bool),
            typeof(int?),
            typeof(long?),
            typeof(double?),
            typeof(bool?),
            typeof(DateTime),
            typeof(byte[]),
            typeof(byte?[]),
            typeof(Guid?),
            typeof(Guid),
            typeof(TimeSpan),
            typeof(IPEndPoint),
            typeof(Nullable),
            typeof(Enum)
        };
        [JsonIgnore]
        public TimeSpan? ExpireTime { get; private set; }
        [JsonIgnore]
        public string FullKey => BuildFullKey();
        [JsonIgnore]
        public string SearchKey => BuildSearchKey();
        private string BuildFullKey()
        {
            string fullKey = null;
            var properties = GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(RedisKeyAttribute), false).Where(a => a is RedisKeyAttribute).Any());
            if (properties.Count() == 0)
            {
                throw new ArgumentNullException($"The RedisKeyValueObject:{this.GetType().Name} must have a key");
            }
            foreach (var property in properties)
            {
                if (!_supportedTypes.Contains(property.PropertyType))
                {
                    throw new NotSupportedException("The complex type is not supported");
                }
                if (property.GetValue(this) == null)
                {
                    throw new ArgumentNullException("When building FullKey, every property with RedisKeyAttribute can not be null");
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