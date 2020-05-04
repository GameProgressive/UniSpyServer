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

        public string RemoteQueryReportIP { get; protected set; }
        public string RemoteQueryReportPort { get; protected set; }

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

        public static List<string> GetMatchedKeys(string subKey)
        {
            return RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.GameServer);
        }

        public static bool DeleteGameServer(IPAddress address, string gameName)
        {
            string subKey = address.ToString() + "*" + gameName;
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.GameServer);
            return redis.KeyDelete(subKey);
        }

        public static bool DeleteServer(string key)
        {
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.GameServer);
            return redis.KeyDelete(key);
        }

        public static bool DeleteServer(EndPoint endPoint, string gameName)
        {
            string key = GenerateKey(endPoint, gameName);
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.GameServer);
            return redis.KeyDelete(key);
        }

        public static string GenerateKey(EndPoint end, string gameName)
        {
            return ((IPEndPoint)end).ToString() + " " + gameName;
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
    }
}
