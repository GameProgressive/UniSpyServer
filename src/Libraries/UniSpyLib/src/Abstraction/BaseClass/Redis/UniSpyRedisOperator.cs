using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis
{
    /// <summary>
    /// This class uses generate type for easily convert Redis objects to your ideal objects.
    /// You must construct the search key or full key on your inherited class.
    /// </summary>
    /// <typeparam name="TValue">The serialization type</typeparam>
    public abstract class UniSpyRedisOperator<TKey, TValue>
    {
        public static bool SetKeyValue(UniSpyRedisKey key, TValue value)
        {
            var jsonStr = JsonConvert.SerializeObject(value);
            return RedisExtensions.SetKeyValue(key.FullKey, jsonStr, key.Db);
        }

        public static TValue GetSpecificValue(UniSpyRedisKey key)
        {
            var value = RedisExtensions.GetSpecificValue(key.FullKey, key.Db);
            return value == null ? default(TValue) : JsonConvert.DeserializeObject<TValue>(value);
        }

        public static bool DeleteKeyValue(UniSpyRedisKey key)
        {
            return RedisExtensions.DeleteKeyValue(key.FullKey, key.Db);
        }

        public static List<TKey> GetMatchedKeys(UniSpyRedisKey key)
        {
            var rawKeys = RedisExtensions.GetMatchedKeys(key.SearchKey, key.Db);
            var keys = new List<TKey>();
            rawKeys.ForEach(k => keys.Add(JsonConvert.DeserializeObject<TKey>(k)));
            return keys;
        }

        public static Dictionary<TKey, TValue> GetMatchedKeyValues(UniSpyRedisKey key)
        {
            var keyValuePairs = RedisExtensions.GetMatchedKeyValues(key.SearchKey, key.Db);
            var newDict = keyValuePairs.ToDictionary(
                k => JsonConvert.DeserializeObject<TKey>(k.Key),
                k => JsonConvert.DeserializeObject<TValue>(k.Value));
            return newDict;
        }

        //public static List<TKey> GetAllKeys()
        //{
        //    var keyList = RedisExtensions.GetAllKeys();
        //    List<TKey> keys = new List<TKey>();
        //    foreach (var k in keyList)
        //    {
        //        keys.Add(JsonConvert.DeserializeObject<TKey>(k));
        //    }
        //    return keys;
        //}

        //public static Dictionary<TKey, TValue> GetAllKeyValues()
        //{
        //    var keyValuePairs = RedisExtensions.GetAllKeyValues();
        //    var newDict = keyValuePairs.ToDictionary(
        //        k => JsonConvert.DeserializeObject<TKey>(k.Key),
        //        k => JsonConvert.DeserializeObject<TValue>(k.Value));
        //    return newDict;
        //}
    }
}
