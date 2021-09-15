using NatNegotiation.Entity.Structure.Request;
using System;
using System.Net;

namespace NatNegotiation.Entity.Structure.Redis
{
    internal sealed class UserInfo
    {
        public IPEndPoint RemoteEndPoint { get; set; }
        public InitRequest InitRequestInfo { get; set; }
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        public UserInfo()
        {
        }
    }
}
