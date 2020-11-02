using System;
namespace NATNegotiation.Entity.Structure.Request
{
    public class ErtAckRequest : InitRequest
    {
        public ErtAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
