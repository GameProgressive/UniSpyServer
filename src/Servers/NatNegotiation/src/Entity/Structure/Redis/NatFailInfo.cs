using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.MiscMethod;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis.Fail
{
    /// <summary>
    /// The information pair using to switch strategy
    /// </summary>
    /// <value></value>
    public record NatFailInfo : RedisKeyValueObject
    {
        [RedisKey]
        [JsonConverter(typeof(IPAddresConverter))]
        public IPAddress PublicIPAddress1 { get; init; }
        [RedisKey]
        [JsonConverter(typeof(IPAddresConverter))]
        public IPAddress PublicIPAddress2 { get; init; }
        public NatFailInfo(NatInitInfo info1, NatInitInfo info2) : base(TimeSpan.FromDays(1))
        {
            // we need to store in sequence to make consistancy and reduce duplications
            if (info1.ClientIndex == NatClientIndex.GameClient)
            {
                PublicIPAddress1 = info1.PublicIPEndPoint.Address;
                PublicIPAddress2 = info2.PublicIPEndPoint.Address;
            }
            else
            {
                PublicIPAddress1 = info2.PublicIPEndPoint.Address;
                PublicIPAddress2 = info1.PublicIPEndPoint.Address;
            }
        }
    }

    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<NatFailInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatFailInfo) { }
    }
}