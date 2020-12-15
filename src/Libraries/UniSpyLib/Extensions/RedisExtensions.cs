using UniSpyLib.Abstraction.BaseClass;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace UniSpyLib.Extensions
{
    public enum RedisDBNumber
    {
        PeerGroup = 0,
        GameServer = 1,
        NatNeg = 2
    }

    public class RedisExtensions
    {
        public static bool SetKeyValue<T>(string key, T value, RedisDBNumber dbNumber, TimeSpan? timeSpan = null)
        {
            var redis = UniSpyServerManagerBase.Redis.GetDatabase((int)dbNumber);
            string jsonStr = JsonConvert.SerializeObject(value);
            return redis.StringSet(key, jsonStr, timeSpan);
        }

        public static T GetValue<T>(string key, RedisDBNumber dbNumber)
        {
            var redis = UniSpyServerManagerBase.Redis.GetDatabase((int)dbNumber);
            T t = JsonConvert.DeserializeObject<T>(redis.StringGet(key));
            return t;
        }

        public static bool DeleteKeyValue(string key, RedisDBNumber dBNumber)
        {
            var redis = UniSpyServerManagerBase.Redis.GetDatabase((int)dBNumber);
            return redis.KeyDelete(key);
        }


        public static List<string> GetAllKeys(RedisDBNumber dbNumber)
        {
            List<string> matchedKeys = new List<string>();

            foreach (var end in UniSpyServerManagerBase.Redis.GetEndPoints())
            {
                var server = UniSpyServerManagerBase.Redis.GetServer(end);
                foreach (var key in server.Keys((int)dbNumber))
                {
                    matchedKeys.Add(key);
                }
            }
            return matchedKeys;
        }

        /// <summary>
        /// Search our sub key in database get full keys which contain sub key
        /// </summary>
        /// <param name="subKey">the substring of a key</param>
        /// <returns></returns>
        public static List<string> GetMatchedKeys(string subKey, RedisDBNumber dbNumber)
        {
            List<string> matchKeys = new List<string>();

            foreach (var endPoint in UniSpyServerManagerBase.Redis.GetEndPoints())
            {
                var server = UniSpyServerManagerBase.Redis.GetServer(endPoint);
                foreach (var key in server.Keys((int)dbNumber, pattern: $"*{subKey}*"))
                {
                    matchKeys.Add(key);
                }
            }
            return matchKeys;
        }

        public static Dictionary<string, T> GetAllKeyValues<T>(RedisDBNumber dBNumber)
        {
            var keys = GetAllKeys(dBNumber);
            var dict = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                dict.Add(key, GetValue<T>(key, dBNumber));
            }
            return dict;
        }
    }
}
