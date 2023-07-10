using System;

namespace UniSpy.Server.GameTrafficRelay
{
    public record NatNegotiationRequest
    {

        public uint Cookie { get; set; }
        public Guid ServerId { get; set; }
        /// <summary>
        /// Gameserver public ip endpoint, used to validate the negotiator's ip
        /// </summary>
        // [JsonConverter(typeof(IPEndPointConverter))]
        public string GameServerIP { get; set; }
        /// <summary>
        /// Gameclient public ip endpoint, used to validate the negotiator's ip
        /// </summary>
        // [JsonConverter(typeof(IPEndPointConverter))]
        public string GameClientIP { get; set; }
    }
    public record NatNegotiationResponse
    {
        public int Port { get; set; }
        public string Message { get; set; }
    }
}