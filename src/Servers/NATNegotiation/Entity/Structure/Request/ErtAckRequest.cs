using System;
namespace NATNegotiation.Entity.Structure.Request
{
    internal class ErtAckRequest : InitRequest
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
