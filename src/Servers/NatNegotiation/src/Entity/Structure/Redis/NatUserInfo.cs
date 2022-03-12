using System;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis
{
    public record NatUserInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; init; }
        [RedisKey]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteIPEndPoint { get; set; }
        [RedisKey]
        public NatServerType? PortType { get; init; }
        [RedisKey]
        public uint? Cookie { get; init; }
        [RedisKey]
        public NatPortMappingScheme? MappingScheme { get; set; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint LocalIPEndPoint { get; init; }
        public DateTime? LastPacketRecieveTime { get; set; }
        public int? RetryNatNegotiationTime { get; set; }
        public byte? UseGamePort { get; init; }
        public byte? ClientIndex { get; init; }
        public bool IsGotConnectPacket { get; set; }


        public NatUserInfo() : base(TimeSpan.FromMinutes(3))
        {
            // _supportedTypes.Add(typeof(NatPortType?));
        }
    }
    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<NatUserInfo>
    {
        public RedisClient() :
        base(ConfigManager.Config.Redis.ConnectionString,
            (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatNeg)
        { }
    }
}