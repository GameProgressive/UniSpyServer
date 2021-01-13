using System;
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Handler.SystemHandler;

namespace NATNegotiation.Entity.Structure
{
    internal sealed class NatUserInfo
    {
        public static NNRedisOperator RedisOperator;
        public string RemoteEndPoint;
        public byte Version;
        public uint Cookie;
        public byte ClientIndex;
        public NatPortType PortType;
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        static NatUserInfo()
        {
            RedisOperator = new NNRedisOperator();
        }

        public NatUserInfo()
        {
        }

        public void UpdateRemoteEndPoint(IPEndPoint endPoint)
        {
            RemoteEndPoint = (endPoint).ToString();
        }

        public void UpdateInitRequestInfo(InitRequest init)
        {
            Version = init.Version;
            Cookie = init.Cookie;
            PortType = init.PortType;
            ClientIndex = init.ClientIndex;
        }
    }
}
