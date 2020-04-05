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

        #region Peer Group Methods


        public static List<string> SearchPeerGroupKeys(string subKey)
        {
            return SearchKeys(subKey, (int)RedisDBNumber.PeerGroup);
        }

        public static T GetGroupsList<T>(string gameName)
        {
            return SerilizeGet<T>(gameName, (int)RedisDBNumber.PeerGroup);
        }

        public static bool SetGroupList<T>(string gameName, T room)
        {
            return SerializeSet(gameName, room, (int)RedisDBNumber.PeerGroup);
        }

        #endregion

        #region Dedicated Server Methods

        public static List<string> SearchDedicateServerKeys(string subKey)
        {
            return SearchKeys(subKey, (int)RedisDBNumber.DedicatedServer);
        }

        public static bool DeleteDedicatedGameServer(EndPoint endPoint, string gameName)
        {
            string key = GenerateDedicatedGameServerKey(endPoint, gameName);
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.DedicatedServer);
            return redis.KeyDelete(key);
        }

        public static string GenerateDedicatedGameServerKey(EndPoint end, string gameName)
        {
            return ((IPEndPoint)end).Address.ToString() + ":" + ((IPEndPoint)end).Port + " " + gameName;
        }

        public static void UpdateDedicatedGameServer<T>(EndPoint end, string gameName, T gameServer)
        {
            string key = GenerateDedicatedGameServerKey(end, gameName);
            SerializeSet(key, gameServer, (int)RedisDBNumber.DedicatedServer);
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

            List<string> allServerKeys = SearchKeys(subKey, (int)RedisDBNumber.DedicatedServer);
            List<T> gameServer = new List<T>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(SerilizeGet<T>(key, (int)RedisDBNumber.DedicatedServer));
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
            List<string> allServerKeys = SearchKeys(subKey, (int)RedisDBNumber.DedicatedServer);
            List<T> gameServer = new List<T>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(SerilizeGet<T>(key, (int)RedisDBNumber.DedicatedServer));
            }
            return gameServer;
        }


        #endregion

        #region Peer Server Methods
        public static List<string> SearchPeerServerKeys(string gameName)
        {
            return SearchKeys(gameName, (int)RedisDBNumber.PeerServer);
        }
        public static bool DeletePeerServer(EndPoint endPoint, string gameName)
        {
            string key = GeneratePeerGameServerKey(endPoint, gameName);
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.PeerServer);
            return redis.KeyDelete(key);
        }

        public static string GeneratePeerGameServerKey(EndPoint end, string gameName)
        {
            return GenerateDedicatedGameServerKey(end, gameName);
        }
        public static void UpdatePeerGameServer<T>(EndPoint end, string gameName, T gameServer)
        {
            string key = GenerateDedicatedGameServerKey(end, gameName);
            SerializeSet(key, gameServer, (int)RedisDBNumber.PeerServer);
        }

        public static List<T> GetPeerGameServers<T>(string subKey)
        {
            List<string> allServerKeys = SearchKeys(subKey, (int)RedisDBNumber.PeerServer);
            List<T> gameServer = new List<T>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(SerilizeGet<T>(key, (int)RedisDBNumber.DedicatedServer));
            }
            return gameServer;
        }
        #endregion
    }
}
