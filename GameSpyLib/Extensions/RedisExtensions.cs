using GameSpyLib.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace GameSpyLib.Extensions
{
    public enum RedisDBNumber
    {
        DedicatedServer = 0,
        PeerGroup = 1,
        PeerServer = 2
    }

    public class RedisExtensions
    {
        #region General Methods
        public static bool SerializeSet<T>(string key, T value, int dbNumber)
        {
            var redis = ServerManagerBase.Redis.GetDatabase(dbNumber);
            string jsonStr = JsonConvert.SerializeObject(value);
            return redis.StringSet(key, jsonStr);
        }

        public static T SerilizeGet<T>(string key, int dbNumber)
        {
            var redis = ServerManagerBase.Redis.GetDatabase(dbNumber);
            T t = JsonConvert.DeserializeObject<T>(redis.StringGet(key));
            return t;
        }

        /// <summary>
        /// Search our sub key in database get full keys which contain sub key
        /// </summary>
        /// <param name="subStringOfKey">the substring of a key</param>
        /// <returns></returns>
        public static List<string> SearchKeys(string subStringOfKey, int dbNumber)
        {
            List<string> matchKeys = new List<string>();

            foreach (var end in ServerManagerBase.Redis.GetEndPoints())
            {
                var server = ServerManagerBase.Redis.GetServer(end);
                foreach (var key in server.Keys(dbNumber, pattern: $"*{subStringOfKey}*"))
                {
                    matchKeys.Add(key);
                }
            }
            return matchKeys;
        }
        #endregion

        public static List<string> SearchPeerServerKeys(string gameName)
        {
            return SearchKeys(gameName, (int)RedisDBNumber.PeerServer);
        }
    }
}
