using System;
using System.Net;
namespace UniSpyServer.Servers.GameTrafficRelay
{
    public record NatNegotiationRequest
    {
        public uint Cookie { get; set; }
        public Guid ServerId { get; set; }
    }
    public record NatNegotiationResponse
    {
        public string IPEndPoint1 { get; set; }
        public string IPEndPoint2 { get; set; }

        public NatNegotiationResponse(IPEndPoint iPEndPoint1, IPEndPoint iPEndPoint2)
        {
            IPEndPoint1 = iPEndPoint1.ToString();
            IPEndPoint2 = iPEndPoint2.ToString();
        }

    }
}