using NATNegotiation.Entity.Structure.Request;
using NATNegotiation.Handler.SystemHandler;
using System;
using System.Net;

namespace NATNegotiation.Entity.Structure
{
    internal sealed class NatUserInfo
    {
        public IPEndPoint RemoteEndPoint { get; set; }
        public InitRequest InitRequestInfo { get; set; }
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        public NatUserInfo()
        {
        }
    }
}
