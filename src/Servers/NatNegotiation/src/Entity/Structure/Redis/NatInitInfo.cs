using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.NatNegotiation.Application;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis
{
    public record NatAddressInfo
    {
        public byte? Version { get; init; }
        public NatPortType? PortType { get; init; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }
        /// <summary>
        /// The nat negotiation private ip and port using as p2p port
        /// nn1,nn2,nn3 is using to detect NAT type
        /// </summary>
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PrivateIPEndPoint { get; init; }
    }
    public record NatInitInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; init; }
        [RedisKey]
        public uint? Cookie { get; init; }
        [RedisKey]
        public byte? Version { get; init; }
        [RedisKey]
        public NatClientIndex? ClientIndex { get; init; }

        /// <summary>
        /// Negotiating flag to set 
        /// </summary>
        public bool isNegotiating { get; set; }
        public int? RetryNatNegotiationTime { get; set; }
        public byte? UseGamePort { get; init; }
        /// <summary>
        /// Is using UniSpyGameRelay to relay game traffic for clients, default value is false
        /// </summary>
        public bool IsUsingRelayServer { get; set; }
        public bool IsReceivedAllPackets => AddressInfos.ContainsKey(NatPortType.NN1)
                                            && AddressInfos.ContainsKey(NatPortType.NN2)
                                            && AddressInfos.ContainsKey(NatPortType.NN3);
        public ConcurrentDictionary<NatPortType, NatAddressInfo> AddressInfos { get; private set; }
        /// <summary>
        /// The public ip for other client connect
        /// </summary>
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint
        {
            get
            {
                if (ClientIndex == NatClientIndex.GameServer && AddressInfos.ContainsKey(NatPortType.GP))
                {
                    return AddressInfos[NatPortType.GP].PublicIPEndPoint;
                }
                else
                {
                    return AddressInfos[NatPortType.NN3].PublicIPEndPoint;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PrivateIPEndPoint
        {
            get
            {
                if (AddressInfos[NatPortType.NN2].PrivateIPEndPoint.Equals(AddressInfos[NatPortType.NN3].PrivateIPEndPoint))
                {
                    return AddressInfos[NatPortType.NN3].PrivateIPEndPoint;
                }
                else
                {
                    throw new NNException("Client is sending wrong initpacket.");
                }
            }
        }

        public NatType NatType { get; internal set; }

        public NatInitInfo() : base(TimeSpan.FromMinutes(3))
        {
            AddressInfos = new ConcurrentDictionary<NatPortType, NatAddressInfo>();
        }
        /// <summary>
        /// Create the key to search in Dictionary
        /// </summary>
        /// <param name="cookie">The cookie of natneg protocol</param>
        /// <param name="clientIndex">The client index of current packet</param>
        /// <param name="version">The version of natneg protocol</param>
        /// <returns>Key string</returns>
        public static string CreateKey(uint cookie, NatClientIndex clientIndex, uint version)
        {
            return $"{cookie} {clientIndex} {version}";
        }
    }

    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<NatInitInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatNeg) { }
    }
}