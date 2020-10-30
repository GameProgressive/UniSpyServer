using GameSpyLib.Abstraction.BaseClass;
using GameSpyLib.Entity.Enumerator;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameSpyLib.Extensions
{
    public enum RedisDBNumber
    {
        PeerGroup = 0,
        GameServer = 1,
        NatNeg = 2
    }

    public class RedisExtensions
    {
        public static bool SerializeSet<T>(string key, T value, RedisDBNumber dbNumber)
        {
            var redis = ServerManagerBase.Redis.GetDatabase((int)dbNumber);
            string jsonStr = JsonConvert.SerializeObject(value);
            return redis.StringSet(key, jsonStr);
        }

        public static T SerilizeGet<T>(string key, RedisDBNumber dbNumber)
        {
            var redis = ServerManagerBase.Redis.GetDatabase((int)dbNumber);
            T t = JsonConvert.DeserializeObject<T>(redis.StringGet(key));
            return t;
        }

        public static List<string> GetAllKeys(RedisDBNumber dbNumber)
        {
            List<string> matchKeys = new List<string>();

            foreach (var end in ServerManagerBase.Redis.GetEndPoints())
            {
                var server = ServerManagerBase.Redis.GetServer(end);
                foreach (var key in server.Keys((int)dbNumber))
                {
                    matchKeys.Add(key);
                }
            }
            return matchKeys;
        }

        /// <summary>
        /// Search our sub key in database get full keys which contain sub key
        /// </summary>
        /// <param name="subStringOfKey">the substring of a key</param>
        /// <returns></returns>
        public static List<string> GetMatchedKeys(string subStringOfKey, RedisDBNumber dbNumber)
        {
            List<string> matchKeys = new List<string>();

            foreach (var end in ServerManagerBase.Redis.GetEndPoints())
            {
                var server = ServerManagerBase.Redis.GetServer(end);
                foreach (var key in server.Keys((int)dbNumber, pattern: $"*{subStringOfKey}*"))
                {
                    matchKeys.Add(key);
                }
            }
            return matchKeys;
        }
    }
}
