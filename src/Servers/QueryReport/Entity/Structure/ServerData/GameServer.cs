using GameSpyLib.Common;
using GameSpyLib.Extensions;
using QueryReport.Entity.Structure.ReportData;
using System;
using System.Collections.Generic;
using System.Net;

namespace QueryReport.Entity.Structure
{
    /// <summary>
    /// This is the server 
    /// </summary>
    public class GameServer
    {
        /// <summary>
        /// Last valid heart beat packet time
        /// </summary>
        public DateTime LastPacket { get; set; }

        public string RemoteQueryReportIP { get; set; }
        public string RemoteQueryReportPort { get; set; }

        public ServerData ServerData { get; set; }
        public PlayerData PlayerData { get; set; }
        public TeamData TeamData { get; set; }

        public GameServer()
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
        }

        public void Parse(EndPoint endPoint)
        {
            RemoteQueryReportIP = ((IPEndPoint)endPoint).Address.ToString();
            RemoteQueryReportPort = ((IPEndPoint)endPoint).Port.ToString();
        }

        public static List<string> GetSimilarKeys(string subKey)
        {
            return RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.GameServer);
        }

        public static List<string> GetSimilarKeys(EndPoint endPoint, string gameName)
        {
            string address = ((IPEndPoint)endPoint).Address.ToString();
            return GetSimilarKeys($"{address}*{gameName}");
        }
        public static bool DeleteSimilarServer(EndPoint endPoint, string gameName)
        {
            string address = ((IPEndPoint)endPoint).Address.ToString();
            string subKey = address + "*" + gameName;
            List<string> keys = GetSimilarKeys(subKey);
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.GameServer);
            foreach (var key in keys)
            {
                DeleteSpecificServer(key);
            }

            return true;
        }

        public static bool DeleteSpecificServer(string key)
        {
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.GameServer);
            return redis.KeyDelete(key);
        }

        public static bool DeleteSpecificServer(EndPoint endPoint, string gameName)
        {
            string key = GenerateKey(endPoint, gameName);
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.GameServer);
            return redis.KeyDelete(key);
        }

        public static string GenerateKey(EndPoint end, string gameName)
        {
            return $"{(IPEndPoint)end} {gameName}";
        }

        public static void UpdateServer(EndPoint end, string gameName, GameServer gameServer)
        {
            string key = GenerateKey(end, gameName);
            RedisExtensions.SerializeSet(key, gameServer, RedisDBNumber.GameServer);
        }

        /// <summary>
        /// Search dedicated game server by its endpoint
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<GameServer> GetServers(EndPoint end)
        {
            //we build search key as 192.168.1.1:1111 format
            string subKey = ((IPEndPoint)end).ToString();

            List<string> allServerKeys = RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.GameServer);
            List<GameServer> gameServer = new List<GameServer>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(RedisExtensions.SerilizeGet<GameServer>(key, RedisDBNumber.GameServer));
            }
            return gameServer;
        }
        /// <summary>
        /// Search game server by sub key
        /// </summary>
        /// <param name="subKey"></param>
        /// <returns></returns>
        public static List<GameServer> GetServers(string subKey)
        {
            List<string> allServerKeys = RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.GameServer);
            List<GameServer> gameServer = new List<GameServer>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(RedisExtensions.SerilizeGet<GameServer>(key, RedisDBNumber.GameServer));
            }
            return gameServer;
        }

        public static Dictionary<string, GameServer> GetAllServers()
        {
            var allServerKeys = RedisExtensions.GetAllKeys(RedisDBNumber.GameServer);
            Dictionary<string, GameServer> gameServer = new Dictionary<string, GameServer>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(key, RedisExtensions.SerilizeGet<GameServer>(key, RedisDBNumber.GameServer));
            }
            return gameServer;
        }
    }
}
