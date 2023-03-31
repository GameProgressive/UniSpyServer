using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using UniSpy.LinqToRedis;
using UniSpy.Server.QueryReport.Enumerate;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Misc;
using UniSpy.Server.Core.Extension.Redis;
using System.Linq;

namespace UniSpy.Server.QueryReport.Aggregate.Redis.GameServer
{
    public record GameServerInfo : LinqToRedis.RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; set; }
        [RedisKey]
        [JsonConverter(typeof(IPAddresConverter))]
        public IPAddress HostIPAddress { get; set; }
        [RedisKey]
        public uint? InstantKey { get; set; }
        [RedisKey]
        public string GameName { get; set; }
        /// <summary>
        /// The port for send heartbeat to query report server.
        /// When a client want to connect to this game server, Server browser will send natneg message to this port.👌
        /// Must set this as nullable object, otherwise it will have default value
        /// </summary>
        [RedisKey]
        public ushort? QueryReportPort { get; set; }
        [JsonIgnore]
        public IPEndPoint QueryReportIPEndPoint => new IPEndPoint(HostIPAddress, (int)QueryReportPort);
        [JsonIgnore]
        /// <summary>
        /// Get the hton bytes of query report port, easy for access
        /// </summary>
        public byte[] QueryReportPortBytes => BitConverter.GetBytes((ushort)QueryReportPort).Reverse().ToArray();
        public DateTime LastPacketReceivedTime { get; set; }
        // public IPEndPoint RemoteQueryReportIPEndPoint { get; set; }
        public GameServerStatus ServerStatus { get; set; }
        public Dictionary<string, string> ServerData { get; set; }
        public List<Dictionary<string, string>> PlayerData { get; set; }
        public List<Dictionary<string, string>> TeamData { get; set; }
        public GameServerInfo() : base(TimeSpan.FromSeconds(30))
        {
        }
    }
    public class RedisClient : LinqToRedis.RedisClient<GameServerInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)RedisDbNumber.GameServerV2)
        {
        }
    }
}