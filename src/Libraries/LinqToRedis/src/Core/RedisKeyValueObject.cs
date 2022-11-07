using System;
using System.Collections.Generic;
using System.Linq;
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


            var propDict = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                propDict.Add(property.Name, property.GetValue(this));
            }
            if (propDict.Values.ToList()[0] is null)
            {
                builder.Append("*");
            }
            for (int i = 0; i < propDict.Keys.Count; i++)
            {
                if (propDict.Values.ToList()[i] is not null)
                {
                    builder.Append($"{propDict.Keys.ToList()[i]}={propDict.Values.ToList()[i]}");

                    // determine whether two non-null property is close to each other, if yes then do not add * , if no add *
                    if (propDict.Values.ToList()[i + 1] is null)
                    {
                        builder.Append("*");
                    }
                    else
                    {
                        if (i != propDict.Keys.Count)
                        {
                            builder.Append(":");
                        }
                    }
                }
            }
            // determine whether last item is null, if yes add * 
            if (propDict.Values.ToList()[propDict.Values.Count - 1] is null)
            {
                builder.Append("*");
            }

            // for (int i = 0; i < properties.Count; i++)
            // {
            //     // determine whether if property is first object
            //     var property = properties[i];
            //     if (property.GetValue(this) is null)
            //     {
            //         continue;
            //     }
            //     else
            //     {

            //         builder.Append($"{property.Name}={property.GetValue(this)}");
            //     }
            //     builder.Append("*");
            // }

            return builder.ToString();
        }
    }
}