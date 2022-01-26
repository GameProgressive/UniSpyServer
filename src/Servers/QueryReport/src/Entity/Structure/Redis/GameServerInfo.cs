using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer
{
    public record GameServerInfo : LinqToRedis.RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; set; }
        [RedisKey]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteIPEndPoint { get; set; }
        [RedisKey]
        public int? InstantKey { get; set; }
        [RedisKey]
        public string GameName { get; set; }
        public DateTime LastPacketReceivedTime { get; set; }
        public IPEndPoint RemoteQueryReportIPEndPoint { get; set; }
        public GameServerStatus ServerStatus;
        public Dictionary<string, string> ServerData { get; set; }
        public List<Dictionary<string, string>> PlayerData { get; set; }
        public List<Dictionary<string, string>> TeamData { get; set; }
        public GameServerInfo() : base(TimeSpan.FromMinutes(3))
        {
        }
    }
    public class RedisClient : LinqToRedis.RedisClient<GameServerInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.ConnectionString, (int)DbNumber.GameServer)
        {
        }
    }
}