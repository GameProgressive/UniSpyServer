using System.Collections.Generic;
using System;

namespace UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay
{
    public record NatNegotiationRequest
    {
        public uint Cookie { get; set; }
        public Guid ServerId { get; set; }
        /// <summary>
        /// Gameserver public ip endpoint, used to validate the negotiator's ip
        /// </summary>
        public List<string> GameServerIPs { get; set; }
        /// <summary>
        /// Gameclient public ip endpoint, used to validate the negotiator's ip
        /// </summary>
        public List<string> GameClientIPs { get; set; }
    }
    public record NatNegotiationResponse
    {
        public int Port { get; set; }
        public string Message { get; set; }
    }
}