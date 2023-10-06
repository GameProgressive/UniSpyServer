using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpy.LinqToRedis.Linq;

namespace UniSpy.LinqToRedis
{
    /// <summary>
    /// TODO we need to implement get AllKeys, AllValues
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class RedisClient<TValue> where TValue : RedisKeyValueObject
    {
        public TimeSpan? ExpireTime { get; private set; }
        public IConnectionMultiplexer Multiplexer { get; private set; }
        public IDatabase Db { get; private set; }
        protected EndPoint[] _endPoints => Multiplexer.GetEndPoints();
        private RedisQueryProvider<TValue> _provider;
        /// <summary>
        /// Search redis key value storage by key
        /// </summary>
        /// <typeparam name="TKey">The redis key class</typeparam>
        /// <returns></returns>
        public QueryableObject<TValue> Context;
        /// <summary>
        /// The default keyvalue object, used to provide default information
        /// </summary>
        public readonly TValue DefaultKVObject = ((TValue)System.Activator.CreateInstance(typeof(TValue)));
        public RedisClient(string connectionString) : this(ConnectionMultiplexer.Connect(connectionString)) { }
        /// <summary>
        /// Use existing multiplexer for performance
        /// </summary>
        public RedisClient(IConnectionMultiplexer multiplexer)
        {
            CheckValidation();
            Multiplexer = multiplexer;
            Db = Multiplexer.GetDatabase((int)DefaultKVObject.Db);
            _provider = new RedisQueryProvider<TValue>(this);
            Context = new QueryableObject<TValue>(_provider);
        }
        private void CheckValidation()
        {
            // check if Db is set
            if (DefaultKVObject.Db is null)
            {
                throw new ArgumentNullException($"The RedisKeyValueObject:{this.GetType().Name} must set database number in constructor");
            }
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

        public async Task DeleteKeyValueAsync(TValue key)
        {
            await Db.KeyDeleteAsync(key.FullKey);
        }
        public void DeleteKeyValue(TValue key)
        {
            Db.KeyDeleteAsync(key.FullKey);
        }
        public List<TValue> GetValues(TValue key)
        {
            return GetKeyValues(key).Values.ToList();
        }
        public async Task<List<TValue>> GetValuesAsync(TValue key)
        {
            var dict = await GetKeyValuesAsync(key);
            return dict.Values.ToList();
        }
        public List<string> GetMatchedKeys(IRedisKey key = null)
        {
            var matchedKeys = new List<string>();
            var searchKey = key is null ? DefaultKVObject.SearchKey : key.SearchKey;
            foreach (var end in _endPoints)
            {
                var server = Multiplexer.GetServer(end);
                // we get matched key from database
                foreach (var k in server.Keys(pattern: searchKey, database: Db.Database))
                {
                    matchedKeys.Add(k);
                }
            }
            return matchedKeys;
        }
        public async Task<List<string>> GetMatchedKeysAsync(IRedisKey key = null)
        {
            var matchedKeys = new List<string>();
            var searchKey = key is null ? DefaultKVObject.SearchKey : key.SearchKey;
            foreach (var end in _endPoints)
            {
                var server = Multiplexer.GetServer(end);
                // we get specific key from database
                await foreach (var k in server.KeysAsync(pattern: searchKey, database: Db.Database))
                {
                    matchedKeys.Add(k);
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
                var value = Db.StringGet(k.ToString());
                dict.Add(k, JsonConvert.DeserializeObject<TValue>(value));
            }
            return dict;
        }

        public async Task<Dictionary<string, TValue>> GetKeyValuesAsync(IRedisKey key = null)
        {
            var dict = new Dictionary<string, TValue>();
            var keys = await GetMatchedKeysAsync(key);
            foreach (var k in keys)
            {
                var value = await Db.StringGetAsync(k.ToString());
                dict.Add(k, JsonConvert.DeserializeObject<TValue>(value));
            }
            return dict;
        }

        public bool SetValue(TValue value)
        {
            return Db.StringSet(value.FullKey, JsonConvert.SerializeObject((TValue)value), value.ExpireTime);
        }
        public async Task<bool> SetValueAsync(TValue value)
        {
            return await Db.StringSetAsync(value.FullKey, JsonConvert.SerializeObject((TValue)value), value.ExpireTime);
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
        public async Task<TValue> GetValueAsync(IRedisKey key)
        {
            var value = await Db.StringGetAsync(key.FullKey);
            if (value.IsNull)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<TValue>(value);
        }

        /// <summary>
        /// The index access of redis key value object is always sync
        /// </summary>
        public TValue this[IRedisKey key]
        {
            get => GetValue(key);
            set => SetValue(value);
        }
        public async Task FlushDbAsync()
        {
            var keys = GetMatchedKeysAsync();
            foreach (var key in await keys)
            {
                await Db.KeyDeleteAsync(key);
            }
        }
        public void FlushDb()
        {
            var keys = GetMatchedKeys();
            foreach (var key in keys)
            {
                Db.KeyDelete(key);
            }
        }
    }
}
