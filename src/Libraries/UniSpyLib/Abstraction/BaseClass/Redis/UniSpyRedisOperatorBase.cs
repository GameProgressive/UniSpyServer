using Newtonsoft.Json;
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
    /// <typeparam name="T">The serialization type</typeparam>
    public abstract class UniSpyRedisOperatorBase<T>
    {
        /// <summary>
        /// Give value to this inside child class.
        /// </summary>
        public static RedisDataBaseNumber? _dbNumber;
        public static TimeSpan? _timeSpan;

        public static bool SetKeyValue(UniSpyRedisKeyBase key, T value)
        {
            var keyStr = key.BuildFullKey();
            return SetKeyValue(keyStr, value);
        }

        public static bool SetKeyValue(string key, T value)
        {
            var jsonStr = JsonConvert.SerializeObject(value);
            return RedisExtensions.SetKeyValue(key, jsonStr, _dbNumber);
        }
        public static T GetSpecificValue(UniSpyRedisKeyBase key)
        {
            var keyStr = key.BuildFullKey();
            return GetSpecificValue(keyStr);
        }

        public static bool DeleteKeyValue(UniSpyRedisKeyBase key)
        {
            var keyStr = key.BuildFullKey();
            return DeleteKeyValue(keyStr);
        }

        public static List<string> GetMatchedKeys(UniSpyRedisKeyBase key)
        {
            var keyStr = key.BuildSearchKey();
            return RedisExtensions.GetMatchedKeys(keyStr, _dbNumber);
        }

        public static List<string> GetAllKeys()
        {
            return RedisExtensions.GetAllKeys(_dbNumber);
        }

        public static Dictionary<string, T> GetMatchedKeyValues(UniSpyRedisKeyBase key)
        {
            var keyStr = key.BuildSearchKey();
            var keyValuePairs = RedisExtensions.GetMatchedKeyValues(keyStr, _dbNumber);
            var newDict = keyValuePairs.ToDictionary(k => k.Key, k => JsonConvert.DeserializeObject<T>(k.Value));
            return newDict;
        }

        public static Dictionary<string, T> GetAllKeyValues()
        {
            var kv = RedisExtensions.GetAllKeyValues(_dbNumber);
            var newDict = kv.ToDictionary(k => k.Key, k => JsonConvert.DeserializeObject<T>(k.Value));
            return newDict;
        }

        public static T GetSpecificValue(string key)
        {
            var value = RedisExtensions.GetSpecificValue(key, _dbNumber);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        public static bool DeleteKeyValue(string key)
        {
            return RedisExtensions.DeleteKeyValue(key, _dbNumber);
        }
    }
}
