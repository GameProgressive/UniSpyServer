using System;
using System.Collections.Generic;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass.Factory;

namespace UniSpyLib.Extensions
{
    public enum RedisDataBaseNumber
    {
        PeerGroup = 0,
        GameServer = 1,
        NatNeg = 2,
        GamePresence = 3
    }

    /// <summary>
    /// This class wraps functions that allow us to access
    /// the string representation of keys and values in Redis database
    /// </summary>
    public class RedisExtensions
    {
        public static bool SetKeyValue(string key, string value, RedisDataBaseNumber? dbNumber, TimeSpan? timeSpan = null)
        {
            var redis = UniSpyServerFactory.Redis.GetDatabase((int)dbNumber);
            return redis.StringSet(key, value, timeSpan);
        }

        /// <summary>
        /// Get only single value from database by specific key
        /// </summary>
        /// <param name="fullKey">The specific key</param>
        /// <param name="dbNumber">database number</param>
        /// <returns></returns>
        public static string GetSpecificValue(string fullKey, RedisDataBaseNumber? dbNumber)
        {
            var redis = UniSpyServerFactory.Redis.GetDatabase((int)dbNumber);
            var matchedKeys = GetMatchedKeys(fullKey, dbNumber);
            if (matchedKeys.Count == 0)
            {
                return null;
            }
            else if (matchedKeys.Count == 1)
            {
                return redis.StringGet(fullKey);
            }
            else
            {
                throw new Exception("There exist similar keys, please check redis database!");
            }
        }

        public static bool DeleteKeyValue(string key, RedisDataBaseNumber? dBNumber)
        {
            var redis = UniSpyServerFactory.Redis.GetDatabase((int)dBNumber);
            return redis.KeyDelete(key);
        }


        public static List<string> GetAllKeys(RedisDataBaseNumber? dbNumber)
        {
            List<string> matchedKeys = new List<string>();

            foreach (var end in UniSpyServerFactory.Redis.GetEndPoints())
            {
                var server = UniSpyServerFactory.Redis.GetServer(end);
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
        public static List<string> GetMatchedKeys(string subKey, RedisDataBaseNumber? dbNumber)
        {
            List<string> matchKeys = new List<string>();

            foreach (var endPoint in UniSpyServerFactory.Redis.GetEndPoints())
            {
                var server = UniSpyServerFactory.Redis.GetServer(endPoint);
                foreach (var key in server.Keys(database: (int)dbNumber, pattern: subKey))
                {
                    matchKeys.Add(key);
                }
            }
            return matchKeys;
        }

        public static Dictionary<string, string> GetMatchedKeyValues(string subKey, RedisDataBaseNumber? dbNumber)
        {
            var dict = new Dictionary<string, string>();
            var matchedKeys = GetMatchedKeys(subKey, dbNumber);
            foreach (var key in matchedKeys)
            {
                dict.Add(key, GetSpecificValue(key, dbNumber));
            }
            return dict;
        }


        public static Dictionary<string, string> GetAllKeyValues(RedisDataBaseNumber? dbNumber)
        {
            var keys = GetAllKeys(dbNumber);
            var dict = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                dict.Add(key, GetSpecificValue(key, dbNumber));
            }
            return dict;
        }
    }
}
