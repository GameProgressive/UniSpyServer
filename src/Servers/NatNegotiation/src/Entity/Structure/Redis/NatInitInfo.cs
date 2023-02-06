using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Exception;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.MiscMethod;
using UniSpyServer.Servers.NatNegotiation.Handler.CmdHandler;
using System.Linq;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis
{
    public record NatAddressInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; init; }
        [RedisKey]
        public uint? Cookie { get; init; }
        [RedisKey]
        public byte? Version { get; init; }
        [RedisKey]
        public NatPortType? PortType { get; init; }
        [RedisKey]
        public NatClientIndex? ClientIndex { get; init; }
        public bool UseGamePort { get; init; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }
        /// <summary>
        /// The nat negotiation private ip and port using as p2p port
        /// nn1,nn2,nn3 is using to detect NAT type
        /// </summary>
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PrivateIPEndPoint { get; init; }
        public NatAddressInfo() : base(TimeSpan.FromMinutes(3))
        {
        }
    }
    public record NatInitInfo
    {
        public uint Cookie => (uint)AddressInfos[NatPortType.GP].Cookie;
        public byte Version => (byte)AddressInfos[NatPortType.GP].Version;
        public NatClientIndex ClientIndex => (NatClientIndex)AddressInfos[NatPortType.GP].ClientIndex;
        public bool UseGamePort => AddressInfos[NatPortType.GP].UseGamePort;
        public bool IsReceivedAllPackets => AddressInfos.ContainsKey(NatPortType.GP)
                                            && AddressInfos.ContainsKey(NatPortType.NN1)
                                            && AddressInfos.ContainsKey(NatPortType.NN2)
                                            && AddressInfos.ContainsKey(NatPortType.NN3);
        public Dictionary<NatPortType, NatAddressInfo> AddressInfos { get; private set; }
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

        public NatType NatType
        {
            get
            {
                return AddressCheckHandler.DetermineNatType(this).NatType;
            }
        }

        public NatInitInfo(List<NatAddressInfo> infos)
        {
            AddressInfos = infos.Select((i) => new { i }).ToDictionary(a => ((NatPortType)a.i.PortType), a => a.i);
            if (!IsReceivedAllPackets)
            {
                throw new NNException("Incomplete init packets");
            }
            if (AddressInfos[NatPortType.GP].Cookie != AddressInfos[NatPortType.NN1].Cookie
                || AddressInfos[NatPortType.GP].Cookie != AddressInfos[NatPortType.NN2].Cookie
                || AddressInfos[NatPortType.GP].Cookie != AddressInfos[NatPortType.NN3].Cookie)
            {
                throw new NNException("Broken cookie");
            }
            if (AddressInfos[NatPortType.GP].Version != AddressInfos[NatPortType.NN1].Version
                || AddressInfos[NatPortType.GP].Version != AddressInfos[NatPortType.NN2].Version
                || AddressInfos[NatPortType.GP].Version != AddressInfos[NatPortType.NN3].Version)
            {
                throw new NNException("Broken version");
            }

            if (AddressInfos[NatPortType.GP].ClientIndex != AddressInfos[NatPortType.NN1].ClientIndex
               || AddressInfos[NatPortType.GP].ClientIndex != AddressInfos[NatPortType.NN2].ClientIndex
               || AddressInfos[NatPortType.GP].ClientIndex != AddressInfos[NatPortType.NN3].ClientIndex)
            {
                throw new NNException("Broken client index");
            }
            if (AddressInfos[NatPortType.GP].UseGamePort != AddressInfos[NatPortType.NN1].UseGamePort
                || AddressInfos[NatPortType.GP].UseGamePort != AddressInfos[NatPortType.NN2].UseGamePort
                || AddressInfos[NatPortType.GP].UseGamePort != AddressInfos[NatPortType.NN3].UseGamePort)
            {
                throw new NNException("Broken use game port");
            }
        }
    }

    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<NatAddressInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatAddressInfo) { }
    }
}