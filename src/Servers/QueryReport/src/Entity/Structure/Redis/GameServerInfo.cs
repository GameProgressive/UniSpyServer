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
        [JsonConverter(typeof(IPAddresConverter))]
        public IPAddress HostIPAddress { get; set; }
        [RedisKey]
        public uint? InstantKey { get; set; }
        [RedisKey]
        public string GameName { get; set; }
        /// <summary>
        /// The port for send heartbeat to query report server.
        /// When a client want to connect to this game server, Server browser will send natneg message to this port.👌
        /// </summary>
        /// <value></value>
        [RedisKey]
        public ushort? QueryReportPort { get; set; }
        [JsonIgnore]
        public IPEndPoint QueryReportIPEndPoint => new IPEndPoint(HostIPAddress, (int)QueryReportPort);
        public DateTime LastPacketReceivedTime { get; set; }
        // public IPEndPoint RemoteQueryReportIPEndPoint { get; set; }
        public GameServerStatus ServerStatus;
        public Dictionary<string, string> ServerData { get; set; }
        public List<Dictionary<string, string>> PlayerData { get; set; }
        public List<Dictionary<string, string>> TeamData { get; set; }
        public GameServerInfo() : base(TimeSpan.FromSeconds(30))
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