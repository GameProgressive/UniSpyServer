using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure
{
    public class ClientInfo : ClientInfoBase
    {
        /// <summary>
        /// The dictionary to store the init results
        /// </summary>
        /// <value></value>
        public uint? Cookie { get; set; }
        public bool? IsIPRestricted { get; set; }
        public bool? IsPortRestricted { get; set; }
        public NatType? NatType { get; set; }
        public NatPortMappingScheme? PortMappingScheme { get; set; }
        // public NatPromiscuity? Promiscuity { get; set; }
        public IPEndPoint PrivateIPEndPoint { get; set; }
        public IPEndPoint PublicIPEndPoint { get; set; }
        public DateTime? LastPacketRecieveTime { get; set; }
        public int? RetryNatNegotiationTime { get; set; }
        public byte? UseGamePort { get; set; }
        public byte? ClientIndex { get; set; }
        public bool? IsGotConnectPacket { get; set; }
        public int NegotiationRetryTimes { get; set; }
        /// <summary>
        /// If max retry time is reached, we use natneg server to transit game data
        /// </summary>
        public bool? IsTransitTraffic => NegotiationRetryTimes == 4;
        /// <summary>
        /// If all nat punch through not working, we enable natneg server as redirector
        /// for transitting the network traffic for each user
        /// </summary>
        /// <value></value>
        public Client TrafficTransitTarget { get; set; }
        public IPEndPoint GuessedPublicIPEndPoint { get; set; }
        public ClientInfo()
        {
            NegotiationRetryTimes = 0;
        }
    }
}