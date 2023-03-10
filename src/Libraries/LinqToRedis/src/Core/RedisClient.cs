using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpy.LinqToRedis.Linq;

namespace UniSpy.LinqToRedis
{
    /// <summary>
    /// TODO we need to implement get AllKeys, AllValues
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class RedisClient<TValue> : IDisposable where TValue : RedisKeyValueObject
    {
        public TimeSpan? ExpireTime { get; private set; }
        public IConnectionMultiplexer Multiplexer { get; private set; }
        public IDatabase Db { get; private set; }
        private EndPoint[] _endPoints => Multiplexer.GetEndPoints();
        private RedisQueryProvider<TValue> _provider;
        /// <summary>
        /// Search redis key value storage by key
        /// </summary>
        /// <typeparam name="TKey">The redis key class</typeparam>
        /// <returns></returns>
        public QueryableObject<TValue> Context;
        // public List<TValue> AllValues => Context.ToList();
        // public List<TValue> AllKeys => Context.Select(k => k.Key).ToList();
        public RedisClient(string connectionString, int db)
        {
            CheckValidation();
            Multiplexer = ConnectionMultiplexer.Connect(connectionString);
            Db = Multiplexer.GetDatabase(db);
            _provider = new RedisQueryProvider<TValue>(this);
            Context = new QueryableObject<TValue>(_provider);
        }
        private void CheckValidation()
        {
            var properties = typeof(TValue).GetProperties().Where(p => p.GetCustomAttributes(typeof(RedisKeyAttribute), true).Select(a => a is RedisKeyAttribute).Any()).ToList();
            if (properties.Count() == 0)
            {
                throw new ArgumentNullException($"The RedisKeyValueObject:{this.GetType().Name} must have a key");
            }
            // we need to check whether 
            var valueProperties = properties.Where(p => p.PropertyType.IsValueType == true && p.PropertyType.IsGenericType == false).ToList();
            if (valueProperties.Count >= 1)
            {
                var propNames = "";
                foreach (var prop in valueProperties)
                {
                    propNames += $" {typeof(TValue).Name}.{prop.Name}";
                }
                throw new ArgumentException($"The RedisKey object must be reference type, please convert the following properties to reference type:{propNames}");
            }
        }
        /// <summary>
        /// Use existing multiplexer for performance
        /// </summary>
        /// <param name="multiplexer"></param>
        /// <param name="db"></param>
        public RedisClient(IConnectionMultiplexer multiplexer, int db)
        {
            CheckValidation();
            Multiplexer = multiplexer;
            Db = Multiplexer.GetDatabase(db);
            _provider = new RedisQueryProvider<TValue>(this);
            Context = new QueryableObject<TValue>(_provider);
        }
        public void DeleteKeyValue(TValue key)
        {
            Db.KeyDeleteAsync(key.FullKey);
        }
        public List<TValue> GetValues(TValue key)
        {
            return GetKeyValues(key).Values.ToList();
        }
        public List<string> GetMatchedKeys(IRedisKey key = null)
        {
            var matchedKeys = new List<string>();
            foreach (var end in _endPoints)
            {
                var server = Multiplexer.GetServer(end);
                if (key is null)
                {
                    // we get all key from database
                    foreach (var k in server.Keys(pattern: "*", database: Db.Database))
                    {
                        matchedKeys.Add(k);
                    }
                }
                else
                {
                    // we get specific key from database
                    foreach (var k in server.Keys(pattern: key.SearchKey, database: Db.Database))
                    {
                        matchedKeys.Add(k);
                    }
                }
            }
            return matchedKeys;
        }
        /// <summary>
        /// Get matched key value by key from database.
        /// if key is null this will get all key value from database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, TValue> GetKeyValues(IRedisKey key = null)
        {
            var dict = new Dictionary<string, TValue>();
            var keys = GetMatchedKeys(key);
            foreach (var k in keys)
            {
                dict.Add(k, JsonConvert.DeserializeObject<TValue>(Db.StringGet(k.ToString())));
            }
            return dict;
        }
        public void SetValue(TValue value)
        {
            // we use async method to make redis set operation do not block our codes
            Db.StringSetAsync(value.FullKey, JsonConvert.SerializeObject((TValue)value), value.ExpireTime);
        }
        public TValue GetValue(IRedisKey key)
        {
            var value = Db.StringGet(key.FullKey);
            if (value.IsNull)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<TValue>(value);
        }
        public TValue this[IRedisKey key]
        {
            get => GetValue(key);
            set => SetValue(value);
        }
        public void Dispose()
        {
            Multiplexer.Dispose();
        }
    }
}
