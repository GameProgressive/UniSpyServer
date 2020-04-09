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
        public DateTime LastHeartBeatPacket { get; set; }

        /// <summary>
        /// Last keep alive packet time
        /// </summary>
        public DateTime LastKeepAlive { get; set; }

        /// <summary>
        /// Last ping packet time
        /// </summary>
        public DateTime LastPing { get; set; }

        public int RemoteIP { get; set; }
        public ushort RemotePort { get; set; }

        /// <summary>
        /// Instant key used to verity the client
        /// </summary>
        public int InstantKey;

        public bool IsValidated = false;


        public ServerData ServerData { get; set; }
        public PlayerData PlayerData { get; set; }
        public TeamData TeamData { get; set; }

        public GameServer()
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
        }

        public void Parse(EndPoint endPoint, int instantKey)
        {
            RemoteIP = BitConverter.ToInt32(((IPEndPoint)endPoint).Address.GetAddressBytes());
            RemotePort = (ushort)((IPEndPoint)endPoint).Port;
            InstantKey = instantKey;
        }

        public static List<string> SearchDedicateServerKeys(string subKey)
        {
            return RedisExtensions.SearchKeys(subKey, (int)RedisDBNumber.DedicatedServer);
        }

        public static bool DeleteGameServer(EndPoint endPoint, string gameName)
        {
            string key = GenerateDedicatedGameServerKey(endPoint, gameName);
            var redis = ServerManagerBase.Redis.GetDatabase((int)RedisDBNumber.DedicatedServer);
            return redis.KeyDelete(key);
        }

        public static string GenerateDedicatedGameServerKey(EndPoint end, string gameName)
        {
            return ((IPEndPoint)end).Address.ToString() + ":" + ((IPEndPoint)end).Port + " " + gameName;
        }

        public static void UpdateGameServer(EndPoint end, string gameName, GameServer gameServer)
        {
            string key = GenerateDedicatedGameServerKey(end, gameName);
            RedisExtensions.SerializeSet(key, gameServer, (int)RedisDBNumber.DedicatedServer);
        }

        /// <summary>
        /// Search dedicated game server by its endpoint
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<GameServer> GetGameServers(EndPoint end)
        {
            //we build search key as 192.168.1.1:1111 format
            string subKey = ((IPEndPoint)end).Address.ToString() + ":" + ((IPEndPoint)end).Port;

            List<string> allServerKeys = RedisExtensions.SearchKeys(subKey, (int)RedisDBNumber.DedicatedServer);
            List<GameServer> gameServer = new List<GameServer>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(RedisExtensions.SerilizeGet<GameServer>(key, (int)RedisDBNumber.DedicatedServer));
            }
            return gameServer;
        }
        /// <summary>
        /// Search game server by sub key
        /// </summary>
        /// <param name="subKey"></param>
        /// <returns></returns>
        public static List<GameServer> GetGameServers(string subKey)
        {
            List<string> allServerKeys = RedisExtensions.SearchKeys(subKey, (int)RedisDBNumber.DedicatedServer);
            List<GameServer> gameServer = new List<GameServer>();
            foreach (var key in allServerKeys)
            {
                gameServer.Add(RedisExtensions.SerilizeGet<GameServer>(key, (int)RedisDBNumber.DedicatedServer));
            }
            return gameServer;
        }
    }
}
