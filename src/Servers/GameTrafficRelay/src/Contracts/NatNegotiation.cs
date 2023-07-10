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
        /// <summary>
        /// Gameserver public ip endpoint, used to validate the negotiator's ip
        /// </summary>
        [JsonConverter(typeof(IPEndPoint))]
        public IPEndPoint GameServerEnd { get; set; }
        /// <summary>
        /// Gameclient public ip endpoint, used to validate the negotiator's ip
        /// </summary>
        [JsonConverter(typeof(IPEndPoint))]
        public IPEndPoint GameClientEnd { get; set; }
    }
    public record NatNegotiationResponse
    {
        [JsonConverter(typeof(IPEndPointConverter))]
        public int Port { get; set; }
        public string Message { get; set; }
    }
}