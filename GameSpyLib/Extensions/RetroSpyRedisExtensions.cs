using GameSpyLib.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace GameSpyLib.Extensions
{
    public class RetroSpyRedisExtensions
    {
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
        public static List<string> SearchKeys(string subStringOfKey)
        {
            List<string> matchKeys = new List<string>();
            var ends = ServerManagerBase.Redis.GetEndPoints();
            foreach (var end in ends)
            {
                var server = ServerManagerBase.Redis.GetServer(end);
                foreach (var key in server.Keys(pattern: "*" + subStringOfKey + "*"))
                {
                    matchKeys.Add(key);
                }
            }
            return matchKeys;
        }

        public static bool DeleteDedicatedGameServer(EndPoint endPoint, string gameName)
        {
            string key = BuildDedicatedGameServerKey(endPoint, gameName);
            var redis = ServerManagerBase.Redis.GetDatabase(0);
            return redis.KeyDelete(key);
        }

        public static string BuildDedicatedGameServerKey(EndPoint end, string gameName)
        {
            return ((IPEndPoint)end).Address.ToString() + ":" + ((IPEndPoint)end).Port + " " + gameName;
        }

        public static void UpdateDedicatedGameServer<T>(EndPoint end, string gameName, T gameServer)
        {
            string key = BuildDedicatedGameServerKey(end, gameName);
            SerializeSet(key, gameServer, 0);
        }

        /// <summary>
        /// Search dedicated game server by its endpoint
        /// </summary>
        /// <typeparam name="T">game server</typeparam>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<T> GetDedicatedGameServers<T>(EndPoint end)
        {
            //we build search key as 192.168.1.1:1111 format
            string subKey = ((IPEndPoint)end).Address.ToString() + ":" + ((IPEndPoint)end).Port;

            List<string> allServerKeys = SearchKeys(subKey);
            List<T> gameServer = new List<T>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(SerilizeGet<T>(key, 0));
            }
            return gameServer;
        }
        /// <summary>
        /// Search game server by sub key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subKey"></param>
        /// <returns></returns>
        public static List<T> GetDedicatedGameServers<T>(string subKey)
        {
            List<string> allServerKeys = SearchKeys(subKey);
            List<T> gameServer = new List<T>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(SerilizeGet<T>(key, 0));
            }
            return gameServer;
        }

        public static T GetGroupsList<T>(string gameName)
        {
            return SerilizeGet<T>(gameName, 1);
        }

        public static void SetGroupList<T>(string gameName, T room)
        {
            SerializeSet(gameName, room, 1);
        }
    }
}
