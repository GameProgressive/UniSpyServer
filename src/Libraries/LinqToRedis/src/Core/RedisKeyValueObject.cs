using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace UniSpy.LinqToRedis
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
        [RedisKey]
        public int? Db { get; private set; }
        public RedisKeyValueObject(int db, TimeSpan? expireTime = null)
        {
            ExpireTime = expireTime;
            Db = db;
        }
        /// <summary>
        /// This is using for json deserialization
        /// </summary>
        protected RedisKeyValueObject() { }

        private string BuildFullKey()
        {
            string fullKey = null;
            var properties = GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(RedisKeyAttribute), true).Select(a => a is RedisKeyAttribute).Any()).ToList();

            foreach (var property in properties)
            {
                // if (!_supportedTypes.Contains(property.PropertyType))
                // {
                //     throw new NotSupportedException($"The complex type:{property.PropertyType} is not supported");
                // }
                if (property.GetValue(this) is null)
                {
                    throw new ArgumentNullException($"{property.Name} is null when building full key.");
                }
                var keyValueStr = $"{property.Name}={property.GetValue(this)}";
                if (fullKey is null)
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
            var properties = GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(RedisKeyAttribute), false).Where(a => a is RedisKeyAttribute).Any()).ToList();
            if (properties.Count() == 0)
            {
                throw new ArgumentNullException($"The RedisKeyValueObject:{this.GetType().Name} must have a key");
            }


            var propKVList = new List<KeyValuePair<string, object>>();
            foreach (var property in properties)
            {
                propKVList.Add(new KeyValuePair<string, object>(property.Name, property.GetValue(this)));
            }

            foreach (var item in propKVList)
            {
                if (item.Value is null)
                {
                    builder.Append($"{item.Key}=*");
                }
                else
                {
                    builder.Append($"{item.Key}={item.Value}");
                }

                if (item.Key != propKVList.Last().Key)
                {
                    builder.Append(":");
                }
            }
            return builder.ToString();
        }
    }
}