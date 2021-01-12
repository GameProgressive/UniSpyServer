using System;
using NATNegotiation.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Request
{
    internal class NatifyRequest : InitRequest
    {
        public NatifyRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
