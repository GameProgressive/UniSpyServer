using System;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.UniSpyLib.MiscMethod;
using UniSpyServer.LinqToRedis;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis
{
    public record UserInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid ServerID { get; set; }
        [RedisKey]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteIPEndPoint { get; set; }
        [RedisKey]
        public NatPortType PortType { get; set; }
        [RedisKey]
        public uint Cookie { get; set; }
        public InitRequest RequestInfo { get; set; }
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        public UserInfo() : base(TimeSpan.FromMinutes(3))
        {
        }
    }
    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<UserInfo>
    {
        public RedisClient() :
        base(ConfigManager.Config.Redis.ConnectionString,
            (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatNeg)
        { }
    }
}