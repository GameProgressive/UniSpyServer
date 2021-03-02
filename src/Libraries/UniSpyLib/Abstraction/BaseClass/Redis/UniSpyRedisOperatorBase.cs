using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Extensions;

namespace UniSpyLib.Abstraction.BaseClass.Redis
{
    /// <summary>
    /// This class uses generate type for easily convert Redis objects to your ideal objects.
    /// You must construct the search key or full key on your inherited class.
    /// </summary>
    /// <typeparam name="TValue">The serialization type</typeparam>
    public abstract class UniSpyRedisOperatorBase<TKey, TValue>
    {
        /// <summary>
        /// Give value to this inside child class.
        /// </summary>
        public static RedisDataBaseNumber? _dbNumber;
        public static TimeSpan? _timeSpan;
        static UniSpyRedisOperatorBase()
        {
        }

        public static bool SetKeyValue(UniSpyRedisKeyBase key, TValue value)
        {
            var jsonStr = JsonConvert.SerializeObject(value);
            return RedisExtensions.SetKeyValue(key.RedisFullKey, jsonStr, _dbNumber);
        }

        public static TValue GetSpecificValue(UniSpyRedisKeyBase key)
        {
            var value = RedisExtensions.GetSpecificValue(key.RedisFullKey, _dbNumber);
            return value == null ? default(TValue) : JsonConvert.DeserializeObject<TValue>(value);
        }

        public static bool DeleteKeyValue(UniSpyRedisKeyBase key)
        {
            return RedisExtensions.DeleteKeyValue(key.RedisFullKey, _dbNumber);
        }

        public static List<TKey> GetMatchedKeys(UniSpyRedisKeyBase key)
        {
            var keyList = RedisExtensions.GetMatchedKeys(key.RedisSearchKey, _dbNumber);
            List<TKey> keys = new List<TKey>();
            foreach (var k in keyList)
            {
                keys.Add(JsonConvert.DeserializeObject<TKey>(k));
            }
            return keys;
        }

        public static List<TKey> GetAllKeys()
        {
            var keyList = RedisExtensions.GetAllKeys(_dbNumber);
            List<TKey> keys = new List<TKey>();
            foreach (var k in keyList)
            {
                keys.Add(JsonConvert.DeserializeObject<TKey>(k));
            }
            return keys;
        }

        public static Dictionary<TKey, TValue> GetMatchedKeyValues(UniSpyRedisKeyBase key)
        {
            var keyValuePairs = RedisExtensions.GetMatchedKeyValues(key.RedisSearchKey, _dbNumber);
            var newDict = keyValuePairs.ToDictionary(
                k => JsonConvert.DeserializeObject<TKey>(k.Key),
                k => JsonConvert.DeserializeObject<TValue>(k.Value));
            return newDict;
        }

        public static Dictionary<TKey, TValue> GetAllKeyValues()
        {
            var keyValuePairs = RedisExtensions.GetAllKeyValues(_dbNumber);
            var newDict = keyValuePairs.ToDictionary(
                k => JsonConvert.DeserializeObject<TKey>(k.Key),
                k => JsonConvert.DeserializeObject<TValue>(k.Value));
            return newDict;
        }
    }
}
