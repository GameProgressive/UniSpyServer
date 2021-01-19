using QueryReport.Entity.Structure.ReportData;
using QueryReport.Handler.SystemHandler.Redis;
using System;
using System.Net;

namespace QueryReport.Entity.Structure
{
    /// <summary>
    /// This is the server 
    /// </summary>
    public class GameServerInfo
    {
        public static GameServerInfoRedisOperator RedisOperator;

        static GameServerInfo()
        {
            RedisOperator = new GameServerInfoRedisOperator();
        }
        /// <summary>
        /// Last valid heart beat packet time
        /// </summary>
        public DateTime LastPacket { get; set; }
        public IPEndPoint RemoteQueryReportIPEndPoint { get; set; }
        public string RemoteQueryReportIP { get; set; }
        public string RemoteQueryReportPort { get; set; }
        public int InstantKey { get; set; }

        public ServerData ServerData { get; set; }
        public PlayerData PlayerData { get; set; }
        public TeamData TeamData { get; set; }

        public GameServerInfo()
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
        }

        public GameServerInfo(EndPoint endPoint)
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
            RemoteQueryReportIP = ((IPEndPoint)endPoint).Address.ToString();
            RemoteQueryReportPort = ((IPEndPoint)endPoint).Port.ToString();
        }

        //public static List<string> GetMatchedKeys(EndPoint endPoint, string gameName)
        //{
        //    string address = ((IPEndPoint)endPoint).Address.ToString();
        //    string key = $"{address}*{gameName}";
        //    return GetMatchedKeys(key);
        //}

        //public static List<string> GetMatchedKeys(string subKey)
        //{
        //    return RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.GameServer);
        //}

        //public static bool DeleteMatchedServers(EndPoint endPoint, string gameName)
        //{
        //    List<string> keys = GetMatchedKeys(endPoint, gameName);
        //    foreach (var key in keys)
        //    {
        //        DeleteSpecificServer(key);
        //    }
        //    return true;
        //}

        //public static bool DeleteSpecificServer(EndPoint endPoint, string gameName)
        //{
        //    string key = GenerateKey(endPoint, gameName);
        //    return DeleteSpecificServer(key);
        //}

        //public static bool DeleteSpecificServer(string key)
        //{
        //    return RedisExtensions.DeleteKeyValue(key, RedisDBNumber.GameServer);
        //}



        ///// <summary>
        ///// Generate key like <ip:port gamename>
        ///// </summary>
        ///// <param name="end"></param>
        ///// <param name="gameName"></param>
        ///// <returns></returns>
        //public static string GenerateKey(EndPoint end, string gameName)
        //{
        //    return $"{(IPEndPoint)end} {gameName}";
        //}

        //public static bool UpdateServer(EndPoint end, string gameName, GameServerInfo gameServer)
        //{
        //    string key = GenerateKey(end, gameName);
        //    return RedisExtensions.SetKeyValue(key, gameServer, RedisDBNumber.GameServer);
        //}

        ///// <summary>
        ///// Search dedicated game server by its endpoint
        ///// </summary>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //public static List<GameServerInfo> GetServers(EndPoint end)
        //{
        //    //we build search key as 192.168.1.1:1111 format
        //    string subKey = ((IPEndPoint)end).ToString();
        //    return GetServers(subKey);
        //}
        ///// <summary>
        ///// Search game server by sub key
        ///// </summary>
        ///// <param name="subKey"></param>
        ///// <returns></returns>
        //public static List<GameServerInfo> GetServers(string subKey)
        //{
        //    List<string> matchedKeys = RedisExtensions.GetMatchedKeys(subKey, RedisDBNumber.GameServer);
        //    List<GameServerInfo> gameServer = new List<GameServerInfo>();
        //    foreach (var key in matchedKeys)
        //    {
        //        gameServer.Add(RedisExtensions.GetValue<GameServerInfo>(key, RedisDBNumber.GameServer));
        //    }
        //    return gameServer;
        //}

        //public static Dictionary<string, GameServerInfo> GetAllServersWithKeys()
        //{
        //    return RedisExtensions.GetAllKeyValues<GameServerInfo>(RedisDBNumber.GameServer);
        //}
    }
}
