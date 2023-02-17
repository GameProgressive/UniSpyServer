using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using UniSpy.LinqToRedis;
using UniSpy.Server.NatNegotiation.Entity.Enumerate;
using UniSpy.Server.NatNegotiation.Entity.Exception;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.MiscMethod;
using UniSpy.Server.NatNegotiation.Handler.CmdHandler;
using System.Linq;

namespace UniSpy.Server.NatNegotiation.Entity.Structure.Redis
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
        public uint Cookie => (uint)AddressInfos[NatPortType.NN3].Cookie;
        public byte Version => (byte)AddressInfos[NatPortType.NN3].Version;
        public NatClientIndex ClientIndex => (NatClientIndex)AddressInfos[NatPortType.NN3].ClientIndex;
        /// <summary>
        /// Indicate whether game use the socket create by natneg sdk to communicate,
        /// if game did not use game port, after negotiation the game communication is not established,
        /// there is a port created by game that will be use to communicate
        /// </summary>
        public bool UseGamePort => AddressInfos[NatPortType.NN3].UseGamePort;
        /// <summary>
        /// Some game will not send GP init packet, we use NN3 as default.
        /// </summary>
        public IPEndPoint PublicIPEndPoint => AddressInfos[NatPortType.NN3].PublicIPEndPoint;
        /// <summary>
        /// The private address will show in NN2 and NN3 packets, in here we use private ip in NN3 as default
        /// </summary>
        public IPEndPoint PrivateIPEndPoint => AddressInfos[NatPortType.NN3].PrivateIPEndPoint;
        public NatType NatType => AddressCheckHandler.DetermineNatType(this).NatType;

        public Dictionary<NatPortType, NatAddressInfo> AddressInfos { get; private set; }

        public NatInitInfo(List<NatAddressInfo> infos)
        {
            AddressInfos = infos.Select((i) => new { i }).ToDictionary(a => ((NatPortType)a.i.PortType), a => a.i);
            // todo 
            // some game will not send GP packet to natneg server, currently do not know the reason of it, need more game for analysis.
            // this will happen in GameClient
            if (!(AddressInfos.ContainsKey(NatPortType.NN1)
                && AddressInfos.ContainsKey(NatPortType.NN2)
                && AddressInfos.ContainsKey(NatPortType.NN3)))
            {
                throw new NNException("Incomplete init packets");
            }

            if (AddressInfos[NatPortType.NN1].Cookie != AddressInfos[NatPortType.NN2].Cookie
                || AddressInfos[NatPortType.NN1].Cookie != AddressInfos[NatPortType.NN3].Cookie)
            {
                throw new NNException("Broken cookie");
            }
            if (AddressInfos[NatPortType.NN1].Version != AddressInfos[NatPortType.NN2].Version
                || AddressInfos[NatPortType.NN1].Version != AddressInfos[NatPortType.NN3].Version)
            {
                throw new NNException("Broken version");
            }

            if (AddressInfos[NatPortType.NN1].ClientIndex != AddressInfos[NatPortType.NN2].ClientIndex
               || AddressInfos[NatPortType.NN1].ClientIndex != AddressInfos[NatPortType.NN3].ClientIndex)
            {
                throw new NNException("Broken client index");
            }
            if (AddressInfos[NatPortType.NN1].UseGamePort != AddressInfos[NatPortType.NN2].UseGamePort
                || AddressInfos[NatPortType.NN1].UseGamePort != AddressInfos[NatPortType.NN3].UseGamePort)
            {
                throw new NNException("Broken use game port");
            }
            if (!AddressInfos[NatPortType.NN2].PrivateIPEndPoint.Equals(AddressInfos[NatPortType.NN3].PrivateIPEndPoint))
            {
                throw new NNException("Client is sending wrong initpacket.");
            }
            if (AddressInfos.ContainsKey(NatPortType.GP))
            {
                if (AddressInfos[NatPortType.GP].Cookie != AddressInfos[NatPortType.NN1].Cookie ||
                AddressInfos[NatPortType.GP].Version != AddressInfos[NatPortType.NN1].Version ||
                AddressInfos[NatPortType.GP].ClientIndex != AddressInfos[NatPortType.NN1].ClientIndex ||
                AddressInfos[NatPortType.GP].UseGamePort != AddressInfos[NatPortType.NN1].UseGamePort)
                {
                    throw new NNException("GP packet info is not correct");
                }
            }
        }
    }

    public class RedisClient : UniSpy.LinqToRedis.RedisClient<NatAddressInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)UniSpy.Server.Core.Extensions.DbNumber.NatAddressInfo) { }
    }
}