using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis.Linq;

namespace UniSpyServer.LinqToRedis
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
        static RedisClient()
        {
            ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
        }
        public RedisClient(string connectionString, int db)
        {
            Multiplexer = ConnectionMultiplexer.Connect(connectionString);
            Db = Multiplexer.GetDatabase(db);
            _provider = new RedisQueryProvider<TValue>(this);
            Context = new QueryableObject<TValue>(_provider);
        }

        /// <summary>
        /// Use existing multiplexer for performance
        /// </summary>
        /// <param name="multiplexer"></param>
        /// <param name="db"></param>
        public RedisClient(IConnectionMultiplexer multiplexer, int db)
        {
            Multiplexer = multiplexer;
            Db = Multiplexer.GetDatabase(db);
            _provider = new RedisQueryProvider<TValue>(this);
            Context = new QueryableObject<TValue>(_provider);
        }
        public bool DeleteKeyValue(TValue key)
        {
            return Db.KeyDelete(key.FullKey);
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
        // public bool UpdateValue(TValue value)
        // {
        //     var keys = GetMatchedKeys(value);
        //     if (keys.Count != 1)
        //     {
        //         throw new System.Exception("Update value failed, key not found or more than one key found");
        //     }
        //     return Db.StringSet(keys.First(), JsonConvert.SerializeObject(value));
        // }
        public bool SetValue(TValue value)
        {
            // var keys = GetMatchedKeys(value);
            // if (keys.Count != 0)
            // {
            //     throw new System.Exception("Set value failed, key already exists");
            // }
            var result = Db.StringSet(value.FullKey, JsonConvert.SerializeObject(value));
            if (value.ExpireTime is not null)
            {
                Db.KeyExpire(value.FullKey, value.ExpireTime.Value);
            }
            return result;
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
