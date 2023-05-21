using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using UniSpy.LinqToRedis;
using UniSpy.Server.NatNegotiation.Enumerate;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Misc;
using UniSpy.Server.NatNegotiation.Handler.CmdHandler;
using System.Linq;
using UniSpy.Server.Core.Extension.Redis;

namespace UniSpy.Server.NatNegotiation.Aggregate.Redis
{
    public record NatAddressInfo : UniSpy.Server.Core.Abstraction.BaseClass.RedisKeyValueObject
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
        [RedisKey]
        public string GameName { get; init; }
        public bool UseGamePort { get; init; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }
        /// <summary>
        /// The nat negotiation private ip and port using as p2p port
        /// nn1,nn2,nn3 is using to detect NAT type
        /// </summary>
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PrivateIPEndPoint { get; init; }

        public NatAddressInfo() : base(RedisDbNumber.NatAddressInfo, TimeSpan.FromMinutes(3))
        {
        }
    }
    public record NatInitInfo
    {
        public uint Cookie => (uint)AddressInfos.Values.Last().Cookie;
        public byte Version => (byte)AddressInfos.Values.Last().Version;
        public NatClientIndex ClientIndex => (NatClientIndex)AddressInfos.Values.Last().ClientIndex;
        /// <summary>
        /// Indicate whether game use the socket create by natneg sdk to communicate,
        /// if game did not use game port, after negotiation the game communication is not established,
        /// there is a port created by game that will be use to communicate
        /// </summary>
        public bool UseGamePort => AddressInfos.Values.Last().UseGamePort;
        /// <summary>
        /// Some game will not send GP init packet, we use NN3 as default.
        /// </summary>
        public IPEndPoint PublicIPEndPoint
        {
            get
            {
                if (Version == 2)
                {
                    return AddressInfos[NatPortType.NN2].PublicIPEndPoint;
                }
                else
                {
                    return AddressInfos[NatPortType.NN3].PublicIPEndPoint;
                }
            }
        }
        /// <summary>
        /// The private address will show in NN2 and NN3 packets, in here we use private ip in NN3 as default
        /// </summary>
        public IPEndPoint PrivateIPEndPoint
        {
            get
            {
                if (Version == 2)
                {
                    return AddressInfos[NatPortType.NN2].PrivateIPEndPoint;
                }
                else
                {
                    return AddressInfos[NatPortType.NN3].PrivateIPEndPoint;
                }
            }
        }
        public NatType NatType
        {
            get
            {
                if (Version == 2)
                {
                    return AddressCheckHandler.DetermineNatTypeVersion2(this).NatType;
                }
                else
                {
                    return AddressCheckHandler.DetermineNatTypeVersion3(this).NatType;
                }
            }
        }

        public Dictionary<NatPortType, NatAddressInfo> AddressInfos { get; private set; }

        public NatInitInfo(List<NatAddressInfo> infos)
        {
            AddressInfos = infos.Select((i) => new { i }).ToDictionary(a => ((NatPortType)a.i.PortType), a => a.i);
            if (infos.First().Version == 2)
            {
                if (!(AddressInfos.ContainsKey(NatPortType.NN1)
                    && AddressInfos.ContainsKey(NatPortType.NN2)))
                {
                    throw new NatNegotiation.Exception("Incomplete init packets");
                }

                if (AddressInfos[NatPortType.NN1].Cookie != AddressInfos[NatPortType.NN2].Cookie)
                {
                    throw new NatNegotiation.Exception("Broken cookie");
                }
                if (AddressInfos[NatPortType.NN1].Version != AddressInfos[NatPortType.NN2].Version)
                {
                    throw new NatNegotiation.Exception("Broken version");
                }
                if (AddressInfos[NatPortType.NN1].ClientIndex != AddressInfos[NatPortType.NN2].ClientIndex)
                {
                    throw new NatNegotiation.Exception("Broken client index");
                }
                if (!AddressInfos[NatPortType.NN1].PrivateIPEndPoint.Equals(AddressInfos[NatPortType.NN2].PrivateIPEndPoint))
                {
                    throw new NatNegotiation.Exception("Client is sending wrong initpacket.");
                }
                if (AddressInfos.ContainsKey(NatPortType.GP))
                {
                    if (AddressInfos[NatPortType.GP].Cookie != AddressInfos[NatPortType.NN1].Cookie ||
                    AddressInfos[NatPortType.GP].Version != AddressInfos[NatPortType.NN1].Version ||
                    AddressInfos[NatPortType.GP].ClientIndex != AddressInfos[NatPortType.NN1].ClientIndex ||
                    AddressInfos[NatPortType.GP].UseGamePort != AddressInfos[NatPortType.NN1].UseGamePort)
                    {
                        throw new NatNegotiation.Exception("GP packet info is not correct");
                    }
                }
            }
            else
            {
                // todo 
                // some game will not send GP packet to natneg server, currently do not know the reason of it, need more game for analysis.
                // this will happen in GameClient
                if (!(AddressInfos.ContainsKey(NatPortType.NN1)
                    && AddressInfos.ContainsKey(NatPortType.NN2)
                    && AddressInfos.ContainsKey(NatPortType.NN3)))
                {
                    throw new NatNegotiation.Exception("Incomplete init packets");
                }

                if (AddressInfos[NatPortType.NN1].Cookie != AddressInfos[NatPortType.NN2].Cookie
                    || AddressInfos[NatPortType.NN1].Cookie != AddressInfos[NatPortType.NN3].Cookie)
                {
                    throw new NatNegotiation.Exception("Broken cookie");
                }
                if (AddressInfos[NatPortType.NN1].Version != AddressInfos[NatPortType.NN2].Version
                    || AddressInfos[NatPortType.NN1].Version != AddressInfos[NatPortType.NN3].Version)
                {
                    throw new NatNegotiation.Exception("Broken version");
                }

                if (AddressInfos[NatPortType.NN1].ClientIndex != AddressInfos[NatPortType.NN2].ClientIndex
                   || AddressInfos[NatPortType.NN1].ClientIndex != AddressInfos[NatPortType.NN3].ClientIndex)
                {
                    throw new NatNegotiation.Exception("Broken client index");
                }
                if (AddressInfos[NatPortType.NN1].UseGamePort != AddressInfos[NatPortType.NN2].UseGamePort
                    || AddressInfos[NatPortType.NN1].UseGamePort != AddressInfos[NatPortType.NN3].UseGamePort)
                {
                    throw new NatNegotiation.Exception("Broken use game port");
                }
                if (!AddressInfos[NatPortType.NN2].PrivateIPEndPoint.Equals(AddressInfos[NatPortType.NN3].PrivateIPEndPoint))
                {
                    throw new NatNegotiation.Exception("Client is sending wrong initpacket.");
                }
                if (AddressInfos.ContainsKey(NatPortType.GP))
                {
                    if (AddressInfos[NatPortType.GP].Cookie != AddressInfos[NatPortType.NN1].Cookie ||
                    AddressInfos[NatPortType.GP].Version != AddressInfos[NatPortType.NN1].Version ||
                    AddressInfos[NatPortType.GP].ClientIndex != AddressInfos[NatPortType.NN1].ClientIndex ||
                    AddressInfos[NatPortType.GP].UseGamePort != AddressInfos[NatPortType.NN1].UseGamePort)
                    {
                        throw new NatNegotiation.Exception("GP packet info is not correct");
                    }
                }
            }
        }
    }

    public class RedisClient : UniSpy.Server.Core.Abstraction.BaseClass.RedisClient<NatAddressInfo>
    {
        public RedisClient()
        {
        }
    }
}