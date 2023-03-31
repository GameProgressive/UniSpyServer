using System;
using System.Net;
using Newtonsoft.Json;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.GameTrafficRelay
{
    public record NatNegotiationRequest
    {
        public uint Cookie { get; set; }
        public Guid ServerId { get; set; }
    }
    public record NatNegotiationResponse
    {
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint IPEndPoint1 { get; set; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint IPEndPoint2 { get; set; }
    }
}