using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Handler.SystemHandler;
using System;
using System.Net;

namespace NATNegotiation.Entity.Structure
{
    internal sealed class NatUserInfo
    {
        public static NNRedisOperator RedisOperator { get; private set; }
        public IPEndPoint RemoteEndPoint { get; set; }
        public InitRequest InitRequestInfo { get; set; }
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        static NatUserInfo()
        {
            RedisOperator = new NNRedisOperator();
        }

        public NatUserInfo()
        {
        }
    }
}
